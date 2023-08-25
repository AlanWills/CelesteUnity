using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace CelesteEditor.Tools
{
    public static class SpriteAtlasConverter
    {

        [MenuItem("Assets/Repack Atlas to Sprite", true)]
        private static bool RepackAtlasToSprite()
        {
            return Selection.activeObject.GetType() == typeof(SpriteAtlas);
        }

        [MenuItem("Celeste/Tools/Assets/Repack Atlas to Sprite")]
        public static void RepackAtlasToSprite(MenuCommand command)
        {
            SpriteAtlas atlas = (SpriteAtlas)Selection.activeObject;
            var padding = atlas.GetPackingSettings().padding;

            //Pack sprites
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            var rects = new List<SpriteRect>();
            foreach (var spr in sprites)
            {
                rects.Add(new SpriteRect(spr, padding));
            }
            rects.Sort((a, b) => b.area.CompareTo(a.area));

            var packer = new RectanglePacker();

            foreach (var rect in rects)
            {
                if (!packer.Pack(rect.w, rect.h, out rect.x, out rect.y))
                    throw new Exception("Uh oh, we couldn't pack the rectangle :(");
            }

            //Calculate image size
            var maxSize = atlas.GetPlatformSettings("DefaultTexturePlatform").maxTextureSize;
            var pngSize = Math.Max(packer.Width, packer.Height);
            var powoftwo = 16;
            while (powoftwo < pngSize) powoftwo *= 2;
            pngSize = powoftwo;
            if (pngSize > maxSize) pngSize = maxSize;
            Texture2D texture = new Texture2D(pngSize, pngSize, TextureFormat.RGBA32, false);

            //Make texture transparent
            Color fillColor = Color.clear;
            Color[] fillPixels = new Color[texture.width * texture.height];
            for (int i = 0; i < fillPixels.Length; i++) fillPixels[i] = fillColor;
            texture.SetPixels(fillPixels);

            var metas = new List<SpriteMetaData>();

            //Draw sprites
            foreach (var rect in rects)
            {
                var t = GetReadableTexture(rect.sprite.texture);
                texture.SetPixels32(rect.x + padding, rect.y + padding, (int)rect.sprite.rect.width, (int)rect.sprite.rect.height, t.GetPixels32());
                metas.Add(new SpriteMetaData()
                {
                    alignment = 6, //BottomLeft
                    name = rect.sprite.name.Replace("(Clone)", ""),
                    rect = new Rect(rect.x + padding, rect.y + padding, rect.sw, rect.sh)
                });
            }

            //Save image
            var path = AssetDatabase.GetAssetPath(atlas);
            var pngPath = path.Replace(".spriteatlas", ".png");

            Debug.Log($"Create sprite from atlas: {atlas.name} path: {path}");

            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(pngPath, bytes);

            //Update sprite settings
            AssetDatabase.Refresh();

            TextureImporter ti = AssetImporter.GetAtPath(pngPath) as TextureImporter;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Multiple;
            ti.spritesheet = metas.ToArray();

            EditorUtility.SetDirty(ti);
            ti.SaveAndReimport();

        }

        private static Texture2D GetReadableTexture(Texture2D source)
        {

            RenderTexture tmp = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, tmp);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;
            Texture2D result = new Texture2D(source.width, source.height);
            result.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            result.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmp);
            return result;
        }
    }

    public class SpriteRect
    {
        public Sprite sprite;
        public int x, y, w, h;

        public SpriteRect(Sprite sprite, int padding)
        {
            this.sprite = sprite;
            this.x = 0;
            this.y = 0;
            this.w = (int)sw + padding * 2;
            this.h = (int)sh + padding * 2;
        }

        public int sw => (int)sprite.rect.width;
        public int sh => (int)sprite.rect.height;
        public int area => w * h;
    }

    // https://github.com/mikaturunen/RectanglePacker
    public class RectanglePacker
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        List<Node> nodes = new List<Node>();

        public RectanglePacker()
        {
            nodes.Add(new Node(0, 0, int.MaxValue, int.MaxValue));
        }

        public bool Pack(int w, int h, out int x, out int y)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                if (w <= nodes[i].W && h <= nodes[i].H)
                {
                    var node = nodes[i];
                    nodes.RemoveAt(i);
                    x = node.X;
                    y = node.Y;
                    int r = x + w;
                    int b = y + h;
                    nodes.Add(new Node(r, y, node.Right - r, h));
                    nodes.Add(new Node(x, b, w, node.Bottom - b));
                    nodes.Add(new Node(r, b, node.Right - r, node.Bottom - b));
                    Width = Math.Max(Width, r);
                    Height = Math.Max(Height, b);
                    return true;
                }
            }
            x = 0;
            y = 0;
            return false;
        }

        public struct Node
        {
            public int X;
            public int Y;
            public int W;
            public int H;

            public Node(int x, int y, int w, int h)
            {
                X = x;
                Y = y;
                W = w;
                H = h;
            }

            public int Right
            {
                get { return X + W; }
            }

            public int Bottom
            {
                get { return Y + H; }
            }
        }
    }

}