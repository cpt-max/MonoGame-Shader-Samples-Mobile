using System;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShaderSample
{
    struct Particle
    {
        public Vector2 pos;
        public Vector2 vel;
    };

    public class ShaderGame : Game
    {
        const int MaxParticleCount = 1000000;
        const int ComputeGroupSize = 256; // has to be the same as the GroupSize define in the compute shader 

        int particleCount = 100000;
        float fps;
        float dt;
        float force;
        Vector2 forceCenter;

        GraphicsDeviceManager graphics;
        Random rand = new Random();
        GUI gui;
        Button forceButton;

        Effect effect;
        SpriteBatch spriteBatch;
        SpriteFont textFont;

        StructuredBuffer particleBuffer; // stores all the particle information, will be updated by the compute shader
        VertexBuffer vertexBuffer; // used for drawing the particles  
        
        public ShaderGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void LoadContent()
        {
            effect = Content.Load<Effect>("Effect");
            textFont = Content.Load<SpriteFont>("TextFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            particleBuffer = new StructuredBuffer(GraphicsDevice, typeof(Particle), MaxParticleCount, BufferUsage.None, ShaderAccess.ReadWrite);
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), MaxParticleCount, BufferUsage.WriteOnly); // no need to initialize, all the data for drawing the particles is coming from the structured buffer

            FillParticlesBufferRandomly();
        }

        public GUI CreateGUI(Android.Content.Context context)
        {
            gui = new GUI(context);

            gui.BeginRow();
            gui.AddButton("Less", Click.Continuous, b => { particleCount -= (int)Math.Ceiling(dt * particleCount); });
            gui.AddButton("More", Click.Continuous, b => { particleCount += (int)Math.Ceiling(dt * particleCount); });
            gui.EndRow();

            gui.BeginRow();
            gui.AddButton("Reset", Click.Single, b => { FillParticlesBufferRandomly(); });
            forceButton = gui.AddButton("Attract", Click.Single, b => { b.Text = b.Text == "Attract" ? "Repulse" : "Attract"; });
            gui.EndRow();

            return gui;
        }

        protected override void Update(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            fps = 1 / dt;

            gui.Update();

            particleCount = Math.Max(1, Math.Min(MaxParticleCount, particleCount));

            var touch = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();
            
            force = touch.Count > 0f ? forceButton.Text == "Attract" ? 1f : -1f : 0f;
            force *= 5;

            if (touch.Count > 0)
            {
                forceCenter = touch[0].Position / new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) * 2 - Vector2.One;
                forceCenter.Y *= -1;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ComputeParticles(gameTime);
            DrawParticles();
            DrawText();

            base.Draw(gameTime);
        }

        private void ComputeParticles(GameTime gameTime)
        {
            effect.Parameters["Particles"].SetValue(particleBuffer);
            effect.Parameters["DeltaTime"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
            effect.Parameters["ForceCenter"].SetValue(forceCenter);
            effect.Parameters["Force"].SetValue(force);

            int groupCount = (int)Math.Ceiling((double)particleCount / ComputeGroupSize);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.ApplyCompute();
                GraphicsDevice.DispatchCompute(groupCount, 1, 1);
            }
        }

        private void DrawParticles()
        {
            effect.Parameters["ParticlesReadOnly"].SetValue(particleBuffer);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SetVertexBuffer(vertexBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, particleCount);
            }
        }

        private void DrawText()
        {
            string text = "FPS\n";
            text += "Particle Count\n";
            text += "Touch for Force Fields\n";

            string values = fps.ToString("0") + "\n";
            values += particleCount.ToString() + "\n";
            values += forceButton.Text + "\n";

            spriteBatch.Begin();
            spriteBatch.DrawString(textFont, text, new Vector2(30, 30), Color.White);
            spriteBatch.DrawString(textFont, values, new Vector2(600, 30), Color.White);
            spriteBatch.End();
        }

        private void FillParticlesBufferRandomly()
        {
            Particle[] particles = new Particle[MaxParticleCount];

            for (int i = 0; i < MaxParticleCount; i++)
            {
                particles[i].pos = new Vector2(
                    (float)rand.NextDouble() * 2 - 1,
                    (float)rand.NextDouble() * 2 - 1);

                particles[i].vel = new Vector2(
                    (float)(rand.NextDouble() * 2 - 1) * 0.1f,
                    (float)(rand.NextDouble() * 2 - 1) * 0.1f);
            }

            particleBuffer.SetData(particles);
        }
    }
}
