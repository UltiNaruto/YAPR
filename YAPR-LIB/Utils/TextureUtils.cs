using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace YAPR_LIB.Utils
{
    public static class TextureUtils
    {
        static readonly int TextureMinX = 42;
        //static readonly int TextureMinY = 1092;
        static readonly int TextureMaxX = 1024;
        static readonly int TextureMaxY = 2048;
        public static UndertaleSprite CreateSprite(UndertaleData gmData, string spriteName, ref int srcX, ref int srcY, int targetW, int targetH, params Image[] images)
        {
            if (images is null || images.Length == 0)
                throw new Exception($"Cannot create sprite {spriteName} with no image!");
            var tex_str = default(UndertaleString);
            var tex = default(UndertaleTexturePageItem);
            var spr = new UndertaleSprite();

            foreach (var img in images)
            {
                if (TextureMaxX - srcX < img.Width)
                {
                    srcY += img.Height;
                    srcX = TextureMinX;
                }
                if (TextureMaxY - srcY < img.Height)
                    throw new Exception("Cannot add newer sprites! Texture page is full!");

                tex_str = new UndertaleString($"PageItem {gmData.TexturePageItems.Count}");
                gmData.Strings.Add(tex_str);

                tex = new UndertaleTexturePageItem();
                tex.Name = tex_str;
                tex.BoundingWidth = (ushort)targetW;
                tex.BoundingHeight = (ushort)targetH;
                tex.SourceX = (ushort)srcX;
                tex.SourceY = (ushort)srcY;
                tex.SourceWidth = (ushort)img.Width;
                tex.SourceHeight = (ushort)img.Height;
                tex.TexturePage = gmData.EmbeddedTextures[0];
                tex.ReplaceTexture(img);
                tex.TargetX -= (ushort)(targetW / 2);
                tex.TargetY -= (ushort)(targetH / 2);
                tex.TargetWidth = (ushort)targetW;
                tex.TargetHeight = (ushort)targetH;

                gmData.TexturePageItems.Add(tex);
                spr.Textures.Add(new UndertaleSprite.TextureEntry()
                {
                    Texture = tex
                });

                srcX += tex.SourceWidth;
            }

            var str = new UndertaleString(spriteName);
            gmData.Strings.Add(str);

            spr.Name = str;
            spr.Transparent = false;
            spr.Preload = false;
            spr.Smooth = false;
            spr.BBoxMode = 0;
            spr.SepMasks = UndertaleSprite.SepMaskType.AxisAlignedRect;
            spr.IsSpecialType = true;
            spr.SVersion = 1;
            spr.GMS2PlaybackSpeed = 10;

            return spr;
        }

        public static UndertaleSprite GetSprite(UndertaleData gmData, string spriteName, int targetW, int targetH, params Rectangle[] src)
        {
            if (src is null || src.Length == 0)
                throw new Exception($"Cannot get sprite {spriteName} with no source coordinates!");
            var tex_str = default(UndertaleString);
            var tex = default(UndertaleTexturePageItem);
            var spr = new UndertaleSprite();

            foreach (var s in src)
            {
                tex_str = new UndertaleString($"PageItem {gmData.TexturePageItems.Count}");
                gmData.Strings.Add(tex_str);

                tex = new UndertaleTexturePageItem();
                tex.Name = tex_str;
                tex.BoundingWidth = (ushort)targetW;
                tex.BoundingHeight = (ushort)targetH;
                tex.SourceX = (ushort)s.X;
                tex.SourceY = (ushort)s.Y;
                tex.SourceWidth = (ushort)s.Width;
                tex.SourceHeight = (ushort)s.Height;
                tex.TexturePage = gmData.EmbeddedTextures[0];
                tex.TargetX -= (ushort)(targetW / 2);
                tex.TargetY -= (ushort)(targetH / 2);
                tex.TargetWidth = (ushort)targetW;
                tex.TargetHeight = (ushort)targetH;

                gmData.TexturePageItems.Add(tex);
                spr.Textures.Add(new UndertaleSprite.TextureEntry()
                {
                    Texture = tex
                });
            }

            var str = new UndertaleString(spriteName);
            gmData.Strings.Add(str);

            spr.Name = str;
            spr.Transparent = false;
            spr.Preload = false;
            spr.Smooth = false;
            spr.BBoxMode = 0;
            spr.SepMasks = UndertaleSprite.SepMaskType.AxisAlignedRect;
            spr.IsSpecialType = true;
            spr.SVersion = 1;
            spr.GMS2PlaybackSpeed = 10;

            return spr;
        }

        public static void ReplaceTexture(this UndertaleTexturePageItem __instance, Image replaceImage, bool disposeImage = true)
        {
            Image finalImage = replaceImage.Clone(x => x.Resize(__instance.SourceWidth, __instance.SourceHeight, KnownResamplers.Lanczos3));

            // Apply the image to the TexturePage.
            lock (__instance.TexturePage.TextureData)
            {
                var texturePageImg = Image.Load(__instance.TexturePage.TextureData.TextureBlob);
                texturePageImg.Mutate(x => x.DrawImage(finalImage, new Point(__instance.SourceX, __instance.SourceY), 1.0f));
                using (var ms = new MemoryStream())
                {
                    texturePageImg.SaveAsPng(ms);
                    __instance.TexturePage.TextureData.TextureBlob = ms.ToArray();
                }
            }

            __instance.TargetWidth = (ushort)replaceImage.Width;
            __instance.TargetHeight = (ushort)replaceImage.Height;

            // Cleanup.
            finalImage.Dispose();
            if (disposeImage)
                replaceImage.Dispose();
        }
    }
}
