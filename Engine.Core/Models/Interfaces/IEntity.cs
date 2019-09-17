using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models.Interfaces
{
    public interface IEntity
    {
        EntityActions EntityLifeCycleAction();
        void ApplySoundEffects(IEnumerable<SoundEffect> soundEffects);
        Task Update(GameTime gameTime);
        Task Render(GameTime gameTime);
        Task Render(GameTime gameTime, Camera camera);
    }
}
