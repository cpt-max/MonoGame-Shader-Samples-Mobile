using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShaderSample
{
    public class ShaderGame : Game
    {
        GraphicsDeviceManager graphics;
        Effect effect;

        public ShaderGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void LoadContent()
        {
            effect = Content.Load<Effect>("Effect");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            var triangle = new VertexPositionTexture[] {
                new VertexPositionTexture(new Vector3( 0,     0.1f, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3( 0.1f, -0.1f, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-0.1f, -0.1f, 0), new Vector2(1, 1)),
            };

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangle, 0, 1);
            }

            base.Draw(gameTime);
        }
    }
}
