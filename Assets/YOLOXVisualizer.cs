/* 
*   YOLOX
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class YOLOXVisualizer : MonoBehaviour {

        #region --Inspector--
        public YOLOXDetection detectionPrefab;
        #endregion


        #region --Client API--
        /// <summary>
        /// Get or set the detection image.
        /// </summary>
        public Texture2D image {
            get => rawImage.texture as Texture2D;
            set {
                rawImage.texture = value;
                aspectFitter.aspectRatio = (float)value.width / value.height;
            }
        }

        /// <summary>
        /// Render a set of object detections.
        /// </summary>
        /// <param name="image">Image which detections are made on.</param>
        /// <param name="detections">Detections to render.</param>
        public void Render (params (Rect rect, string label, float score)[] detections) {
            // Delete current
            foreach (var rect in currentRects)
                GameObject.Destroy(rect.gameObject);
            currentRects.Clear();
            // Render rects
            var imageRect = new Rect(0, 0, image.width, image.height);
            foreach (var detection in detections) {
                var rect = Instantiate(detectionPrefab, transform);
                rect.gameObject.SetActive(true);
                rect.Render(rawImage, detection.rect, detection.label, detection.score);
                currentRects.Add(rect);
            }
        }
        #endregion


        #region --Operations--
        private RawImage rawImage;
        private AspectRatioFitter aspectFitter;
        private readonly List<YOLOXDetection> currentRects = new List<YOLOXDetection>();

        private void Awake () {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
        }
        #endregion
    }
}