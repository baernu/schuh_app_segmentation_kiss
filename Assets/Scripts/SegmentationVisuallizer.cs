using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Mediapipe.BlazePose;
using Mediapipe.SelfieSegmentation;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Color = UnityEngine.Color;
//using OpenCVForUnity.CoreModule;
//using OpenCVForUnity.ImgprocModule;
using Point = OpenCVForUnity.CoreModule.Point;
using UnityEngine.XR.ARSubsystems;


public class PoseVisuallizer3D : MonoBehaviour
{
    //[SerializeField] Camera mainCamera;
    [SerializeField] WebCamInput webCamInput;
    [SerializeField] RawImage inputImageUI;
    //[SerializeField] Shader shader;
    //[SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    //public static Vector4[] landmarks;

    [SerializeField] Material segmentMaterial;
    [SerializeField] RawImage segmentationImage;
    [SerializeField] SelfieSegmentationResource resource;

    SelfieSegmentation segmentation;

    Texture2D segmentedTexture; // segmented mask texture
    public float legHeightRatio = 2f / 3f; // height ratio to start detecting leg

    // Adjust these thresholds based on your segmented mask
    public float colorThreshold = 0.9f; // threshold for considering a pixel as part of the leg
    public int scanWidth = 100; // width of the scanning window

    // Lists to store the detected bone coordinates
    List<Vector2> legright_rightside = new List<Vector2>();
    List<Vector2> legright_leftside = new List<Vector2>();
    public int k = 100;

    //public RawImage rawImage;

    /*
    //Material material;
    BlazePoseDetecter detecter;
    
    // Lines count of body's topology.
    const int BODY_LINE_NUM = 35;
    // Pairs of vertex indices of the lines that make up body's topology.
    // Defined by the figure in https://google.github.io/mediapipe/solutions/pose.
    readonly List<Vector4> linePair = new List<Vector4>{
        new Vector4(0, 1), new Vector4(1, 2), new Vector4(2, 3), new Vector4(3, 7), new Vector4(0, 4),
        new Vector4(4, 5), new Vector4(5, 6), new Vector4(6, 8), new Vector4(9, 10), new Vector4(11, 12),
        new Vector4(11, 13), new Vector4(13, 15), new Vector4(15, 17), new Vector4(17, 19), new Vector4(19, 15),
        new Vector4(15, 21), new Vector4(12, 14), new Vector4(14, 16), new Vector4(16, 18), new Vector4(18, 20),
        new Vector4(20, 16), new Vector4(16, 22), new Vector4(11, 23), new Vector4(12, 24), new Vector4(23, 24),
        new Vector4(23, 25), new Vector4(25, 27), new Vector4(27, 29), new Vector4(29, 31), new Vector4(31, 27),
        new Vector4(24, 26), new Vector4(26, 28), new Vector4(28, 30), new Vector4(30, 32), new Vector4(32, 28)
    };
    */

    void Start()
    {
        //material = new Material(shader);
        //detecter = new BlazePoseDetecter();
        //landmarks = new Vector4[100];
        segmentation = new SelfieSegmentation(resource);
        //segmentedTexture = ToTexture2D(segmentMaterial.mainTexture);
    }

    void Update()
    {
        //mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, 0.1f);
    }


    void LateUpdate()
    {
        inputImageUI.texture = webCamInput.inputImageTexture;

        // Predict pose by neural network model.
        //detecter.ProcessImage(webCamInput.inputImageTexture);

        // Predict segmentation by neural network model.
        segmentation.ProcessImage(webCamInput.inputImageTexture);
        // Visualize segmentation texture as UI image.
        segmentationImage.texture = segmentation.texture;
        segmentMaterial.mainTexture = segmentation.texture;
        DetectRightLeg(segmentation.texture);
        

        /*
        // Output landmark values(33 values) and the score whether human pose is visible (1 values).
        for (int i = 0; i < detecter.vertexCount + 1; i++)
        {
            
            0~32 index datas are pose world landmark.
            Check below Mediapipe document about relation between index and landmark position.
            https://google.github.io/mediapipe/solutions/pose#pose-landmark-model-blazepose-ghum-3d
            Each data factors are
            x, y and z: Real-world 3D coordinates in meters with the origin at the center between hips.
            w: The score of whether the world landmark position is visible ([0, 1]).
        
            33 index data is the score whether human pose is visible ([0, 1]).
            This data is (score, 0, 0, 0).
            
            //Debug.LogFormat("{0}: {1}", i, detecter.GetPoseWorldLandmark(i));
            landmarks[i] = detecter.GetPoseLandmark(i);
        }
        //Debug.Log("---");
        //Debug.Log("landmarks" + landmarks[17]);
        */
    }

    public static void outputLandmarks(Vector4[] landmark)
    {
        //landmark = landmarks;
    }
    /*
    void OnRenderObject()
    {
        // Use predicted pose world landmark results on the ComputeBuffer (GPU) memory.
        material.SetBuffer("_worldVertices", detecter.worldLandmarkBuffer);
        // Set pose landmark counts.
        material.SetInt("_keypointCount", detecter.vertexCount);
        material.SetFloat("_humanExistThreshold", humanExistThreshold);
        material.SetVectorArray("_linePair", linePair);
        material.SetMatrix("_invViewMatrix", mainCamera.worldToCameraMatrix.inverse);

        // Draw 35 world body topology lines.
        material.SetPass(2);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, BODY_LINE_NUM);

        // Draw 33 world landmark points.
        material.SetPass(3);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, detecter.vertexCount);
    }
    */

    void OnApplicationQuit()
    {
        // Must call Dispose method when no longer in use.
        //detecter.Dispose();
        
        segmentation.Dispose();
    }
    
    Texture2D ToTexture2D(Texture texture)
    {
        // Create a new Texture2D with the dimensions of the original texture
        Texture2D texture2D = new Texture2D(texture.width, texture.height);

        // Set the pixels of the new Texture2D from the original texture
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture temporaryRT = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(texture, temporaryRT);
        RenderTexture.active = temporaryRT;
        texture2D.ReadPixels(new UnityEngine.Rect(0, 0, temporaryRT.width, temporaryRT.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = currentActiveRT;
        RenderTexture.ReleaseTemporary(temporaryRT);

        return texture2D;
    }
    bool IsInsideLeg(Vector2 point, Color32[] pixels, int width)
    {
        Color32 pixelColor = pixels[(int)point.y * width + (int)point.x];
        float grayscale = CalculateGrayscale(pixelColor);
        return grayscale > colorThreshold;
    }
    float CalculateGrayscale(Color32 color)
    {
        return (color.r + color.g + color.b) / 3f / 255f; // Calculate grayscale value
    }

    void DetectRightLeg(Texture segmentTex)
    {
        segmentedTexture = ToTexture2D(segmentTex);
        segmentedTexture.Apply();
        //segmentMaterial.mainTexture = segmentedTexture;
        //segmentedTexture.Apply();

        //segmentedTexture = ToTexture2D(segmentMaterial.mainTexture);
        int width = segmentedTexture.width;
        int height = segmentedTexture.height;
        int legHeight = (int)(height * legHeightRatio);
        int count1 = 0; int count2 = 0;
        int value_y = 0;
        Color32[] pixels = segmentedTexture.GetPixels32();
        
        // Iterate 2/3 from the bottom horizontally
        for (int x = legHeight; x < height -k; x+=k)
        {
            // Scan from right to left to find the start of the right leg
            for (int y = width - k; y >= width / 5; y-=30)
            {

                Color32 pixelColor = pixels[x * height + y];
                //Color pixelColor = segmentedTexture.GetPixel(x, y);

                // Check if the pixel color indicates part of the leg
                if (CalculateGrayscale(pixelColor) > colorThreshold)
                {
                    count1++;
                    if (count1 > 1)
                    {
                        legright_rightside.Add(new Vector2(x, y));
                        count1 = 0;value_y = y;
                        break;
                    }
                    
                }
            }

            // Scan from left to middle to find the end of the right leg
            for (int y = value_y; y >= width / 8; y-=30)
            {
                Color pixelColor = segmentedTexture.GetPixel(x, y);

                // Check if the pixel color indicates part of the leg
                if (CalculateGrayscale(pixelColor) < colorThreshold)
                {
                    count2++;
                    if (count2 > 1)
                    {
                        legright_leftside.Add(new Vector2(x, y + 60));
                        count2 = 0;
                        break;
                    }
                }
                
            }

        }


        /*
        // Now you can perform additional operations with the detected coordinates
        // For example, using vectors and factors as you described
        Vector2 a = new Vector2(2, 1);
        Vector2 b = new Vector2(-2, 1);
        

        // Check points in legright_leftside
        foreach (int point in legright_leftside)
        {
            Vector2 currentPoint = new Vector2(point, legright_leftside.IndexOf(point) + legHeight);
            Vector2 nextPoint = currentPoint + a * k;

            // Check if the next point is inside the leg
            if (IsInsideLeg(nextPoint))
            {
                // Begin iterating from outside towards inside
                while (!IsInsideLeg(nextPoint))
                {
                    nextPoint += b * k;
                }

                // Iterate from left to right
                while (IsInsideLeg(nextPoint))
                {
                    legright_leftside.Add((int)nextPoint.x);
                    nextPoint += b * k;
                }

                // Iterate from right to left
                nextPoint = new Vector2(legright_leftside[legright_leftside.Count - 1] + a.x * k, nextPoint.y);
                while (IsInsideLeg(nextPoint))
                {
                    legright_rightside.Insert(0, (int)nextPoint.x);
                    nextPoint -= a * k;
                }
            }
        }*/

        //Debug.Log("list left count: " + legright_leftside.Count);
        //Debug.Log("list left: " + legright_rightside[0].y);
        //Debug.DrawLine(new Vector3(0,0,0), new Vector3(100,0,0), Color.red, 1.0f);
        if (legright_leftside.Count >= legright_rightside.Count)
        {
            for (int i = 1; i < legright_rightside.Count; i++)
            {
                Vector3 leftSide = new Vector3(legright_leftside[i - 1].x, legright_leftside[i - 1].y, -1);
                Vector3 rightSide = new Vector3(legright_rightside[i - 1].x, legright_rightside[i - 1].y, -1);
                Vector3 middle = new Vector3((int)(leftSide.x + rightSide.x) / 2, (int)(leftSide.y + rightSide.y) / 2, -1);
                Vector3 leftSide_next = new Vector3(legright_leftside[i].x, legright_leftside[i].y, -1);
                Vector3 rightSide_next = new Vector3(legright_rightside[i].x, legright_rightside[i].y, -1);
                Vector3 middle_next = new Vector3((int)(leftSide_next.x + rightSide_next.x) / 2, (int)(leftSide_next.y + rightSide_next.y) / 2, -1);

                //DrawLine(segmentedTexture, 0, 0, 300, 300, 50, Color.green);
                DrawLine(segmentedTexture, (int)middle.x, (int)middle.y, (int)middle_next.x, (int)middle_next.y, 100, Color.green);
                //Debug.DrawLine(middle, middle_next, Color.red, 0.1f);
                segmentedTexture.Apply();
                segmentMaterial.mainTexture = segmentedTexture;
                /*
                Scalar color = new Scalar(255, 0, 0);
                Mat outputMat = new Mat(segmentedTexture.height, segmentedTexture.width, CvType.CV_8UC4);
                Imgproc.line(outputMat, new Point(middle.x, middle.y), new Point(middle_next.x, middle_next.y), color , 20);
                Texture2D outputTexture = new Texture2D(outputMat.cols(), outputMat.rows(), TextureFormat.RGBA32, false);
                OpenCVForUnity.UnityUtils.Utils.matToTexture2D(outputMat, outputTexture);
                */
                //segmentMaterial.mainTexture = outputTexture;
                //rawImage.material.mainTexture = outputTexture;
                //rawImage.material.SetTexture("_MainTex", outputTexture);
                //Debug.Log("write a line ...");

            }
        }

        //Debug.Log("list left: ");
        //Debug.Log("list left: " + legright_rightside[0].y);

    }

    bool IsInsideLeg(Vector2 point)
    {
        Color pixelColor = segmentedTexture.GetPixel((int)point.x, (int)point.y);
        return pixelColor.grayscale > colorThreshold;
    }


    void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, int lineWidth, Color a_Color)
    {
        float xPix = x1;
        float yPix = y1;

        float width = x2 - x1;
        float height = y2 - y1;
        float length = Mathf.Abs(width);
        if (Mathf.Abs(height) > length) length = Mathf.Abs(height);
        int intLength = (int)length;
        float dx = width / (float)length;
        float dy = height / (float)length;
        for (int i = 0; i <= intLength; i++)
        {
            a_Texture.SetPixel((int)xPix, (int)yPix, a_Color);

            xPix += dx;
            yPix += dy;
        }
    }
}