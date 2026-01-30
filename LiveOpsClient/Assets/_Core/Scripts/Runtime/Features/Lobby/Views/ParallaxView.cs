using System.Collections.Generic;
using App.Shared.Utils;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class ParallaxView : MonoBehaviour, IParallaxController
    {
        private const int PartsPerLayer = 3;
        private const int CenterPartIndex = 1;

        [SerializeField] private Sprite[] _layers;
        [SerializeField] private SpriteRenderer _layerPrefab;
        [SerializeField] private float _startZ = 10f;
        [SerializeField] private float _layerDepthSpacing = 1f;
        [SerializeField] private float _speed = 1f;

        private readonly List<ParallaxLayer> _parallaxLayers = new();

        private void Awake()
        {
            InitializeLayers();
        }

        private void LateUpdate()
        {
            var movement = Time.deltaTime * _speed;

            foreach (var layer in _parallaxLayers)
            {
                MoveLayerParts(layer, movement);
                WrapAroundParts(layer);
            }
        }
        
        public void Dispose()
            => this.DestroyBehaviourObject();

        private void InitializeLayers()
        {
            for (var layerIndex = 0; layerIndex < _layers.Length; layerIndex++)
            {
                var layer = CreateLayer(_layers[layerIndex], layerIndex);
                _parallaxLayers.Add(layer);
            }
        }

        private ParallaxLayer CreateLayer(Sprite sprite, int layerIndex)
        {
            var layer = new ParallaxLayer();
            var zPosition = _startZ + layerIndex * _layerDepthSpacing;

            for (var partIndex = 0; partIndex < PartsPerLayer; partIndex++)
            {
                var part = CreateLayerPart(sprite, layerIndex, partIndex, zPosition);
                layer.AddPart(part.transform, part.bounds.size.x);
            }

            return layer;
        }

        private SpriteRenderer CreateLayerPart(Sprite sprite, int layerIndex, int partIndex, float zPosition)
        {
            var part = Instantiate(_layerPrefab, transform);
            part.name = $"Layer{layerIndex}_Part{partIndex}";
            part.sprite = sprite;

            var width = sprite.bounds.size.x;
            var xPosition = (partIndex - CenterPartIndex) * width;
            part.transform.localPosition = new Vector3(xPosition, 0f, zPosition);

            return part;
        }

        private void MoveLayerParts(ParallaxLayer layer, float movement)
        {
            foreach (var part in layer.Parts)
                part.Translate(movement * Vector3.right);
        }

        private void WrapAroundParts(ParallaxLayer layer)
        {
            foreach (var part in layer.Parts)
                if (part.localPosition.x > layer.PartWidth)
                    RepositionPartToLeft(part, layer);
        }

        private void RepositionPartToLeft(Transform part, ParallaxLayer layer)
        {
            var leftmostX = FindLeftmostPosition(layer);

            var position = part.localPosition;
            position.x = leftmostX - layer.PartWidth;
            part.localPosition = position;
        }

        private float FindLeftmostPosition(ParallaxLayer layer)
        {
            var leftmostX = float.MaxValue;

            foreach (var part in layer.Parts)
                if (part.localPosition.x < leftmostX)
                    leftmostX = part.localPosition.x;

            return leftmostX;
        }

        private class ParallaxLayer
        {
            public float PartWidth { get; private set; }
            public readonly List<Transform> Parts = new();

            public void AddPart(Transform part, float width)
            {
                Parts.Add(part);
                PartWidth = width;
            }
        }
    }
}