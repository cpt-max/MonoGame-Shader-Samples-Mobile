using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShaderSample
{
    public class ShaderGame : Game
    {
        GUI gui;
        GraphicsDeviceManager graphics;
        Effect effect;
        Texture texture;
        VertexBuffer triangle;
        SpriteBatch spriteBatch;
        SpriteFont textFont;

        int technique = 3;
        float dt;

        float textureDisplacement = 0.1f;
        float tesselation = 8;
        float geometryGeneration = 5;
        float bend;
        
        public ShaderGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void LoadContent()
        {
            effect = Content.Load<Effect>("Effect");
            texture = Content.Load<Texture>("Texture");
            textFont = Content.Load<SpriteFont>("TextFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            triangle = CreateTriangle();
        }

        public GUI CreateGUI(Android.Content.Context context)
        {
            gui = new GUI(context);

            gui.BeginRow();
            gui.AddButton("Bend-", Click.Continuous, b => { bend -= dt * 5; });
            gui.AddButton("Bend+", Click.Continuous, b => { bend += dt * 5; });
            gui.EndRow();

            gui.BeginRow();
            gui.AddButton("Tess-", Click.Continuous, b => { tesselation -= dt * 5; });
            gui.AddButton("Tess+", Click.Continuous, b => { tesselation += dt * 5; });
            gui.EndRow();

            gui.BeginRow();
            gui.AddButton("Geom-", Click.Continuous, b => { geometryGeneration -= dt * 3; });
            gui.AddButton("Geom+", Click.Continuous, b => { geometryGeneration += dt * 3; });
            gui.EndRow();

            gui.BeginRow();
            gui.AddButton("Disp-", Click.Continuous, b => { textureDisplacement -= dt * 1; });
            gui.AddButton("Disp+", Click.Continuous, b => { textureDisplacement += dt * 1; });
            gui.EndRow();

            gui.AddButton("Shader Stages", Click.Single, b => { technique = ++technique % 4; });


            return gui;
        }

        protected override void Update(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            gui.Update();

            bend = Math.Max(-5, Math.Min(5, bend));
            tesselation = Math.Max(1, Math.Min(20, tesselation));
            geometryGeneration = Math.Max(1, Math.Min(10, geometryGeneration));
            textureDisplacement = Math.Max(0, Math.Min(1, textureDisplacement));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix world = Matrix.CreateScale(10);
            Matrix view = Matrix.CreateLookAt(new Vector3(0, -10, 10), new Vector3(0, 0, 8), new Vector3(0, 1, 0));
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, 0.1f, 1000f);

            effect.CurrentTechnique = effect.Techniques[technique];
            effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
            effect.Parameters["Texture"].SetValue(texture);
            effect.Parameters["Bend"].SetValue(bend);
            effect.Parameters["Tesselation"].SetValue(tesselation);
            effect.Parameters["GeometryGeneration"].SetValue(geometryGeneration);
            effect.Parameters["TextureDisplacement"].SetValue(textureDisplacement);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                PrimitiveType primitiveType = hullShaderActive ?
                    PrimitiveType.PatchListWith3ControlPoints :
                    PrimitiveType.TriangleList;

                GraphicsDevice.SetVertexBuffer(triangle);
                GraphicsDevice.DrawPrimitives(primitiveType, 0, triangle.VertexCount / 3);
            }

            DrawText();

            base.Draw(gameTime);
        }

        private void DrawText()
        {
            string text = "Shader Stages: \n";
            text += "Bend: \n";
            text += "Tesselation: \n";
            text += "Geometry Generation: \n";
            text += "Texture Displacement: \n";

            string values = effect.CurrentTechnique.Name + "\n";
            values += (hullShaderActive ? bend.ToString() : "") + "\n";
            values += (hullShaderActive ? tesselation.ToString() : "") + "\n";
            values += (geometryShaderActive ? geometryGeneration.ToString() : "") + "\n";
            values += (geometryShaderActive || hullShaderActive ? textureDisplacement.ToString() : "") + "\n";

            spriteBatch.Begin();
            spriteBatch.DrawString(textFont, text, new Vector2(30, 30), Color.White);
            spriteBatch.DrawString(textFont, values, new Vector2(600, 30), Color.White);
            spriteBatch.End();
        }

        private VertexBuffer CreateTriangle()
        {
            var vertices = new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3( 0, 1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3( 1, 0, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-1, 0, 0), new Vector2(1, 1)),
            };

            var vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
            return vertexBuffer;
        }

        private bool hullShaderActive => effect.CurrentTechnique.Name.Contains("Hull");
        private bool geometryShaderActive => effect.CurrentTechnique.Name.Contains("Geometry");
    }
}
