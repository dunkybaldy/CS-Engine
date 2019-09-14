using Engine.Core.Managers.Interfaces;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace Engine.Core.Managers
{
    public class AssetManager : IAssetManager
    {
        private readonly ContentManager _contentManager;

        public AssetManager(ContentManager contentManager)
        {
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            _contentManager.RootDirectory = "Content";
        }

        public Model GetModel(string assetName)
            => _contentManager.Load<Model>(assetName);

        public Texture2D GetTexture2D(string assetName)
            => _contentManager.Load<Texture2D>(assetName);

        public Texture3D GetTexture3D(string assetName)
            => _contentManager.Load<Texture3D>(assetName);

        public SoundEffect GetSoundEffect(string assetName)
            => _contentManager.Load<SoundEffect>(assetName);

        public Song GetSong(string path)
            => _contentManager.Load<Song>(path);
    }
}
