namespace PaperPlaneTools.AR 
{
	using OpenCvSharp;
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	
	public class ARManager : WebCamera 
	{
		[Serializable]
		public class MarkerObject
		{
			public GameObject markerPrefab;
		}
		 
		/// <summary>
		/// List of possible markers
		/// The list is set in Unity Inspector
		/// </summary>
		public List<MarkerObject> markers;

		/// <summary>
		/// The marker detector
		/// </summary>
		private MarkerDetector markerDetector;

		void Start () 
		{
			markerDetector = new MarkerDetector ();
		}

		protected override void Awake() 
		{
			int cameraIndex = -1;
			for (int i = 0; i < WebCamTexture.devices.Length; i++) 
			{
				WebCamDevice webCamDevice = WebCamTexture.devices [i];
				if (webCamDevice.isFrontFacing == false) 
				{
					cameraIndex = i;
					break;
				}
				if (cameraIndex < 0) 
				{
					cameraIndex = i;
				}
			}

			if (cameraIndex >= 0) 
			{
				DeviceName = WebCamTexture.devices [cameraIndex].name;
			}
		}

		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output) 
		{
			var texture = new Texture2D(input.width, input.height);
			texture.SetPixels(input.GetPixels());
			var img = Unity.TextureToMat(texture, Unity.TextureConversionParams.Default);

			ProcessFrame(img, img.Cols, img.Rows);
			output = Unity.MatToTexture(img, output);

			return true;
		}

		private void ProcessFrame (Mat mat, int width, int height) 
		{
			List<int> markerIds = markerDetector.Detect (mat, width, height);

			int count = 0;
			foreach (MarkerObject markerObject in markers) 
			{
				List<int> foundedMarkers = new List<int>();
				for (int i=0; i<markerIds.Count; i++) 
				{
					foundedMarkers.Add(i);
					count++;
				}

				//Create objects for markers not matched with any game object
				foreach (int markerIndex in foundedMarkers)
				{
					Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
					PositionObject(markerObject.markerPrefab, transforMatrix);
				}
			}
		}

		private void PositionObject(GameObject gameObject, Matrix4x4 transformMatrix) 
		{
			Matrix4x4 matrixY = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, -1, 1));
			Matrix4x4 matrixZ = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, 1, -1));
			Matrix4x4 matrix = matrixY * transformMatrix * matrixZ;

			float z_angle = MatrixHelper.GetQuaternion(matrix).eulerAngles.z;

			// Clamp rotation 
			if (z_angle > 30 && z_angle <= 180)
				z_angle = 30;
			else if (z_angle < 330 && z_angle > 180)
				z_angle = 330;

			gameObject.transform.localRotation = Quaternion.AngleAxis(z_angle, Vector3.forward);
		}
	}
}