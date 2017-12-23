/*
 * Copyright (c) 2017 Yoshihisa Nitta
 * Released under the MIT license
 * http://opensource.org/licenses/mit-license.php
 */

/*
 * NtUnity.cs version 1.2 2017/10/05
 *  http://nw.tsuda.ac.jp/NtKinectDLL/
 *
 * requires:
 *   NtKinectDLL version 1.2.4 or later
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace NtUnity {
  public class Kinect {
    public const int
      // Number
      bodyCount = 6,
      jointCount = 25,
      rgbCols = 1920,
      rgbRows = 1080,
      depthCols = 512,
      depthRows = 424,
      // JointType
      JointType_SpineBase= 0,
      JointType_SpineMid= 1,
      JointType_Neck= 2,
      JointType_Head= 3,
      JointType_ShoulderLeft= 4,
      JointType_ElbowLeft= 5,
      JointType_WristLeft= 6,
      JointType_HandLeft= 7,
      JointType_ShoulderRight= 8,
      JointType_ElbowRight= 9,
      JointType_WristRight= 10,
      JointType_HandRight= 11,
      JointType_HipLeft= 12,
      JointType_KneeLeft= 13,
      JointType_AnkleLeft= 14,
      JointType_FootLeft= 15,
      JointType_HipRight= 16,
      JointType_KneeRight= 17,
      JointType_AnkleRight= 18,
      JointType_FootRight= 19,
      JointType_SpineShoulder= 20,
      JointType_HandTipLeft= 21,
      JointType_ThumbLeft= 22,
      JointType_HandTipRight= 23,
      JointType_ThumbRight= 24,
      // TrackingState
      TrackingState_NotTracked= 0,
      TrackingState_Inferred= 1,
      TrackingState_Tracked= 2,
      // FacePoint
      FacePointType_None= -1,
      FacePointType_EyeLeft= 0,
      FacePointType_EyeRight= 1,
      FacePointType_Nose= 2,
      FacePointType_MouthCornerLeft= 3,
      FacePointType_MouthCornerRight= 4,
      FacePointType_Count= ( FacePointType_MouthCornerRight + 1 ) ,
      // a_FaceProperty
      FaceProperty_Happy= 0,
      FaceProperty_Engaged= 1,
      FaceProperty_WearingGlasses= 2,
      FaceProperty_LeftEyeClosed= 3,
      FaceProperty_RightEyeClosed= 4,
      FaceProperty_MouthOpen= 5,
      FaceProperty_MouthMoved= 6,
      FaceProperty_LookingAway= 7,
      FaceProperty_Count= ( FaceProperty_LookingAway + 1 ) ,
      // FaceDetectionResult
      DetectionResult_Unknown= 0,
      DetectionResult_No= 1,
      DetectionResult_Maybe= 2,
      DetectionResult_Yes= 3,
      // HDFace
      HDFaceVerticesSize = 1347,
      // dummy
      NtKinectdummy = 0;

    [DllImport ("NtKinectDLL")] private static extern IntPtr getKinect();
    [DllImport ("NtKinectDLL")] private static extern void stopKinect(IntPtr ptr);

    // OpenCV
    [DllImport ("NtKinectDLL")] private static extern void imshow(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern void imshowBlack(IntPtr ptr);

    // CoordinateMapper
    [DllImport ("NtKinectDLL")] private static extern void mapCameraPointToColorSpace(IntPtr ptr,IntPtr sv,IntPtr cv,int n);
    [DllImport ("NtKinectDLL")] private static extern void mapCameraPointToDepthSpace(IntPtr ptr,IntPtr sv,IntPtr dv,int n);
    [DllImport ("NtKinectDLL")] private static extern void mapDepthPointToColorSpace(IntPtr ptr,IntPtr dv,IntPtr dth,IntPtr cv,int n);
    [DllImport ("NtKinectDLL")] private static extern void mapDepthPointToCameraSpace(IntPtr ptr,IntPtr dv,IntPtr dth,IntPtr sv,int n);

    // Multi Thread
    [DllImport ("NtKinectDLL")] private static extern void acquire(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern void release(IntPtr ptr);

    // Audio
    [DllImport ("NtKinectDLL")] private static extern void setAudio(IntPtr ptr, bool flag);
    [DllImport ("NtKinectDLL")] private static extern float getBeamAngle(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern float getBeamAngleConfidence(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern ulong getAudioTrackingId(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern void openAudio(IntPtr ptr, IntPtr filename);
    [DllImport ("NtKinectDLL")] private static extern void closeAudio(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern bool isOpenedAudio(IntPtr ptr);

    // RGB
    [DllImport ("NtKinectDLL")] private static extern void setRGB(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getRGB(IntPtr ptr, IntPtr data);

    // Depth
    [DllImport ("NtKinectDLL")] private static extern void setDepth(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getDepth(IntPtr ptr, IntPtr data);

    // Infrared
    [DllImport ("NtKinectDLL")] private static extern void setInfrared(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getInfrared(IntPtr ptr, IntPtr data);

    // BodyIndex
    [DllImport ("NtKinectDLL")] private static extern void setBodyIndex(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getBodyIndex(IntPtr ptr, IntPtr data);
    
    // Skeleton
    [DllImport ("NtKinectDLL")] private static extern void setSkeleton(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getSkeleton(IntPtr ptr, IntPtr skelton, IntPtr state, IntPtr id, IntPtr tid);
    [DllImport ("NtKinectDLL")] private static extern int handState(IntPtr ptr,int id,bool isLeft);
    
    // Face
    [DllImport ("NtKinectDLL")] private static extern void setFace(IntPtr ptr, bool isColorSpace);
    [DllImport ("NtKinectDLL")] private static extern int getFace(IntPtr ptr, IntPtr point,IntPtr rect,IntPtr direction,IntPtr property,IntPtr tid);

    // HDFace
    [DllImport ("NtKinectDLL")] private static extern void setHDFace(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getHDFace(IntPtr ptr, IntPtr point, IntPtr tid, IntPtr status);
    
    // Gesture
    [DllImport ("NtKinectDLL")] private static extern void setGestureFile(IntPtr ptr, IntPtr filename);
    [DllImport ("NtKinectDLL")] private static extern int setGestureId(IntPtr ptr, IntPtr name, int id); // id: non-zero
    [DllImport ("NtKinectDLL")] private static extern void setGesture(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern int getDiscreteGesture(IntPtr ptr, IntPtr gid, IntPtr confidence, IntPtr tid);
    [DllImport ("NtKinectDLL")] private static extern int getContinuousGesture(IntPtr ptr, IntPtr gid, IntPtr progress, IntPtr tid);
    [DllImport ("NtKinectDLL")] private static extern int getGidMapSize();

    // Video
    [DllImport ("NtKinectDLL")] private static extern void openVideo(IntPtr ptr, IntPtr filename);
    [DllImport ("NtKinectDLL")] private static extern void writeVideo(IntPtr ptr);
    [DllImport ("NtKinectDLL")] private static extern void closeVideo(IntPtr ptr);
    
    private IntPtr kinect;
    //public Vector3[] joint = new Vector3[jointCount];
    //public int[] jointState = new int[jointCount];

    // audio
    public float beamAngle;
    public float beamAngleConfidence;
    public ulong audioTrackingId;
    // images
    public Color32[] rgbImage;
    public ushort[] depthImage;
    public ushort[] infraredImage;
    public byte[] bodyIndexImage;
    // skeleton
    public List<List<Vector3>> skeleton;
    public List<List<int>> skeletonState;
    public List<int> skeletonId;
    public List<ulong> skeletonTrackingId;
    // skeleton (internal)
    private float[] skel;
    private int[] skelState;
    private int[] skelId;
    private ulong[] skelTrackingId;
    // face
    public List<List<Vector2>> facePoint;
    public List<Vector4> faceRect;
    public List<Vector3> faceDirection;
    public List<List<int>> faceProperty;
    public List<ulong> faceTrackingId;
    // face (internal)
    private float[] fcPoint;
    private float[] fcRect;
    private float[] fcDirection;
    private int[] fcProperty;
    private ulong[] fcTrackingId;
    // hdface
    public List<List<Vector3>> hdfacePoint;
    public List<ulong> hdfaceTrackingId;
    public List<int> hdfaceStatus;
    // hdface (internal)
    private float[] hdfcPoint;
    private ulong[] hdfcTrackingId;
    private int[] hdfcStatus;
    // gesture
    public List<int> discreteGesture;
    public List<float> gestureConfidence;
    public List<ulong> discreteGestureTrackingId;
    public List<int> continuousGesture;
    public List<float> gestureProgress;
    public List<ulong> continuousGestureTrackingId;
    // gesture (internal)
    private int[] gstId;
    private float[] gstFloat;
    private ulong[] gstTrackingId;
      
    public Kinect() {
      kinect = getKinect();
      //rgbImage = new byte[rgbRows * rgbCols * 4];
      rgbImage = new Color32[rgbRows * rgbCols];
      depthImage = new ushort[depthRows * depthCols];
      infraredImage = new ushort[depthRows * depthCols];
      bodyIndexImage = new byte[depthRows * depthCols];
      skeleton = new List<List<Vector3>>();
      skeletonState = new List<List<int>>();
      skeletonId = new List<int>();
      skeletonTrackingId = new List<ulong>();
      skel = new float[bodyCount * jointCount * 3];
      skelState = new int[bodyCount * jointCount];
      skelId = new int[bodyCount];
      skelTrackingId = new ulong[bodyCount];
      facePoint = new List<List<Vector2>>();
      faceRect = new List<Vector4>();
      faceDirection = new List<Vector3>();
      faceProperty = new List<List<int>>();
      faceTrackingId = new List<ulong>();
      fcPoint = new float[bodyCount * FacePointType_Count * 3];
      fcRect = new float[bodyCount * 4];
      fcDirection = new float[bodyCount * 3];
      fcProperty = new int[bodyCount * FaceProperty_Count];
      fcTrackingId = new ulong[bodyCount];
      hdfacePoint = new List<List<Vector3>>();
      hdfaceTrackingId = new List<ulong>();
      hdfaceStatus = new List<int>();
      hdfcPoint = new float[bodyCount * HDFaceVerticesSize * 3];
      hdfcTrackingId = new ulong[bodyCount];
      hdfcStatus = new int[bodyCount * 2];
      discreteGesture = new List<int>();
      gestureConfidence = new List<float>();
      discreteGestureTrackingId = new List<ulong>();
      continuousGesture = new List<int>();
      gestureProgress = new List<float>();
      continuousGestureTrackingId = new List<ulong>();
      gstId = new int[bodyCount * 100];
      gstFloat = new float[bodyCount * 100];
      gstTrackingId = new ulong[bodyCount * 100];
    }
    public void stopKinect() { stopKinect(kinect); }

    // OpenCV
    public void imshow() { imshow(kinect); }
    public void imshowBlack() { imshowBlack(kinect); }
    
    // coordinateMapper
    public void mapCameraPointToColorSpace(List<Vector3> skel,ref List<Vector2> color,int n) {
      float[] sv = new float[n * 3];
      float[] cv = new float[n * 2];
      for (int i=0; i<n; i++) {
	sv[3*i] = skel[i].x; sv[3*i+1] = skel[i].y; sv[3*i+2] = skel[i].z;
      }
      GCHandle gch = GCHandle.Alloc(sv,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(cv,GCHandleType.Pinned);
      mapCameraPointToColorSpace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),n);
      gch.Free();
      gch2.Free();
      color.Clear();
      for (int i=0; i<n; i++) {
	color.Add(new Vector2(cv[2*i],cv[2*i+1]));
      }
    }
    public void mapCameraPointToDepthSpace(List<Vector3> skel, ref List<Vector2> depth, int n) {
      float[] sv = new float[n * 3];
      float[] dv = new float[n * 2];
      for (int i=0; i<n; i++) {
	sv[3*i] = skel[i].x; sv[3*i+1] = skel[i].y; sv[3*i+2] = skel[i].z;
      }
      GCHandle gch = GCHandle.Alloc(sv,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(dv,GCHandleType.Pinned);
      mapCameraPointToDepthSpace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(), n);
      gch.Free();
      gch2.Free();
      depth.Clear();
      for (int i=0; i<n; i++) {
	depth.Add(new Vector2(dv[2*i],dv[2*i+1]));
      }
    }
    public void mapDepthPointToColorSpace(List<Vector2> depth,ushort[] dth,ref List<Vector2> color,int n) {
      float[] dv = new float[n * 2];
      float[] cv = new float[n * 2];
      for (int i=0; i<n; i++) {
	dv[2*i] = depth[i].x; dv[2*i+1] = depth[i].y;
      }
      GCHandle gch = GCHandle.Alloc(dv,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(dth,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(cv,GCHandleType.Pinned);
      mapDepthPointToColorSpace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject(), n);
      gch.Free();
      gch2.Free();
      gch3.Free();
      color.Clear();
      for (int i=0; i<n; i++) {
	color.Add(new Vector2(cv[2*i],dv[2*i+1]));
      }
    }
    void mapDepthPointToCameraSpace(List<Vector2> depth,ushort[] dth,ref List<Vector3> skel,int n) {
      float[] dv = new float[n * 2];
      float[] sv = new float[n * 3];
      for (int i=0; i<n; i++) {
	dv[2*i] = depth[i].x; dv[2*i+1] = depth[i].y;
      }
      GCHandle gch = GCHandle.Alloc(dv,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(dth,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(sv,GCHandleType.Pinned);
      mapDepthPointToCameraSpace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject(),n);
      gch.Free();
      gch2.Free();
      gch3.Free();
      skel.Clear();
      for (int i=0; i<n; i++) {
	skel.Add(new Vector3(sv[3*i],sv[3*i+1],sv[3*i+2]));
      }
    }

    // Multi Thread
    public void acquire() { acquire(kinect); }
    public void release() { release(kinect); }
    // Audio
    public void setAudio(bool flag) {
      setAudio(kinect,flag);
      beamAngle = getBeamAngle(kinect);
      beamAngleConfidence = getBeamAngleConfidence(kinect);
      audioTrackingId = getAudioTrackingId(kinect);
    }
    /*
    public float getBeamAngle() { return getBeamAngle(kinect); }
    public float getBeamAngleConfidence() { return getBeamAngleConfidence(kinect); }
    */
    public void openAudio(string filename) {
      System.IntPtr fname = Marshal.StringToHGlobalUni(filename);
      openAudio(kinect,fname);
      Marshal.FreeHGlobal(fname);
    }
    public void closeAudio() { closeAudio(kinect); }
    public bool isOpenedAudio() { return isOpenedAudio(kinect); }
    // RGB
    public void setRGB() { setRGB(kinect); getRGB2(); }
    public int getRGB2() {
      GCHandle gch = GCHandle.Alloc(rgbImage,GCHandleType.Pinned);
      int n = getRGB(kinect,gch.AddrOfPinnedObject());
      gch.Free();
      return n;
    }

    // Depth
    public void setDepth() { setDepth(kinect); getDepth(); }
    public int getDepth() {
      GCHandle gch = GCHandle.Alloc(depthImage,GCHandleType.Pinned);
      int n = getDepth(kinect,gch.AddrOfPinnedObject());
      gch.Free();
      return n;
    }

    // Infrared
    public void setInfrared() { setInfrared(kinect); getInfrared(); }
    public int getInfrared() {
      GCHandle gch = GCHandle.Alloc(infraredImage,GCHandleType.Pinned);
      int n = getInfrared(kinect,gch.AddrOfPinnedObject());
      gch.Free();
      return n;
    }

    // BodyIndex
    public void setBodyIndex() {setBodyIndex(kinect); getBodyIndex(); }
    public int getBodyIndex() {
      GCHandle gch = GCHandle.Alloc(bodyIndexImage,GCHandleType.Pinned);
      int n = getBodyIndex(kinect,gch.AddrOfPinnedObject());
      gch.Free();
      return n;
    }

    // Skeleton
    public void setSkeleton() { setSkeleton(kinect); getSkeleton(); }
    public int getSkeleton() {
      GCHandle gch = GCHandle.Alloc(skel,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(skelState,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(skelId,GCHandleType.Pinned);
      GCHandle gch4 = GCHandle.Alloc(skelTrackingId,GCHandleType.Pinned);
      int n = getSkeleton(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject(),gch4.AddrOfPinnedObject());
      gch.Free();
      gch2.Free();
      gch3.Free();
      gch4.Free();
      skeleton.Clear(); skeletonState.Clear(); skeletonId.Clear(); skeletonTrackingId.Clear();
      int idx = 0, st=0;
      for (int i=0; i<n; i++) {
	skeleton.Add(new List<Vector3>());
	skeletonState.Add(new List<int>());
	for (int j=0; j<jointCount; j++) {
	  skeleton[i].Add(new Vector3(skel[idx++], skel[idx++], skel[idx++]));
	  skeletonState[i].Add(skelState[st++]);
	}
	skeletonId.Add(skelId[i]);
	skeletonTrackingId.Add(skelTrackingId[i]);
      }
      return n;
    }

    private int faceFailCount = 0;
    // Face
    public void setFace() { setFace(kinect,true); getFace(); }
    public int getFace() {
      GCHandle gch = GCHandle.Alloc(fcPoint,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(fcRect,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(fcDirection,GCHandleType.Pinned);
      GCHandle gch4 = GCHandle.Alloc(fcProperty,GCHandleType.Pinned);
      GCHandle gch5 = GCHandle.Alloc(fcTrackingId,GCHandleType.Pinned);
      int n = getFace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject(),gch4.AddrOfPinnedObject(),gch5.AddrOfPinnedObject());
      gch.Free();
      gch2.Free();
      gch3.Free();
      gch4.Free();
      gch5.Free();
      if (n == 0) {
	faceFailCount++;
	if (faceFailCount < 10) {
	  return 0;
	} else {
	  faceFailCount = 0;
	}
      }
      facePoint.Clear(); faceRect.Clear(); faceDirection.Clear(); faceProperty.Clear(); faceTrackingId.Clear();
      int idx=0, ridx=0, didx=0, pidx = 0;
      for (int i=0; i<n; i++) {
	facePoint.Add(new List<Vector2>());
	for (int j=0; j<FacePointType_Count; j++) {
	  facePoint[i].Add(new Vector2(fcPoint[idx++],fcPoint[idx++]));
	}
	faceRect.Add(new Vector4(fcRect[ridx++],fcRect[ridx++],fcRect[ridx++],fcRect[ridx++]));
	faceDirection.Add(new Vector3(fcDirection[didx++],fcDirection[didx++],fcDirection[didx++]));
	faceProperty.Add(new List<int>());
	for (int j=0; j<FaceProperty_Count; j++) {
	  faceProperty[i].Add(fcProperty[pidx++]);
	}
	faceTrackingId.Add(fcTrackingId[i]);
      }
      return n;
    }
    public Vector3 getFaceDirectionByTrackingId(ulong tid) {
      for (int i=0; i<faceTrackingId.Count; i++) {
	if (faceTrackingId[i] == tid) {
	  return faceDirection[i];
	}
      }
      return Vector3.zero;
    }

    // HDFace
    public void setHDFace() { setHDFace(kinect); getHDFace(); }
    public int getHDFace() {
      GCHandle gch = GCHandle.Alloc(hdfcPoint,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(hdfcTrackingId,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(hdfcStatus,GCHandleType.Pinned);
      int n = getHDFace(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject());
      gch.Free();
      gch2.Free();
      gch3.Free();
      hdfacePoint.Clear(); hdfaceTrackingId.Clear(); hdfaceStatus.Clear();
      int idx = 0;
      for (int i=0; i<n; i++) {
	hdfacePoint.Add(new List<Vector3>());
	for (int j=0; j < HDFaceVerticesSize; j++) {
	  hdfacePoint[i].Add(new Vector3(hdfcPoint[idx++],hdfcPoint[idx++],hdfcPoint[idx++]));
	}
	hdfaceTrackingId.Add(hdfcTrackingId[i]);
	hdfaceStatus.Add(hdfcStatus[i]);
      }
      return n;
    }

    // Gesture
    public void setGestureFile(string filename) {
      IntPtr gbd = Marshal.StringToHGlobalUni(filename);
      setGestureFile(kinect,gbd);
      Marshal.FreeHGlobal(gbd);
    }
    public int setGestureId(string name, int id) {
      System.IntPtr g = Marshal.StringToHGlobalUni(name); // discrete
      int n = setGestureId(kinect,g,id);
      Marshal.FreeHGlobal(g);
      return n;
    }
    public void setGesture() { setGesture(kinect); }
    public int getDiscreteGesture() {
      int size = bodyCount * getGidMapSize();
      if (gstId.Length < size) {
	gstId = new int[size];
	gstFloat = new float[size];
	gstTrackingId = new ulong[size];
      }
      GCHandle gch = GCHandle.Alloc(gstId,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(gstFloat,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(gstTrackingId,GCHandleType.Pinned);
      int n = getDiscreteGesture(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject());
      gch.Free();
      gch2.Free();
      gch3.Free();
      discreteGesture.Clear(); gestureConfidence.Clear(); discreteGestureTrackingId.Clear();
      for (int i=0; i<n; i++) {
	discreteGesture.Add(gstId[i]);
	gestureConfidence.Add(gstFloat[i]);
	discreteGestureTrackingId.Add(gstTrackingId[i]);
      }
      return n;
    }
    public int getContinuousGesture() {
      int size = bodyCount * getGidMapSize();
      if (gstId.Length < size) {
	gstId = new int[size];
	gstFloat = new float[size];
	gstTrackingId = new ulong[size];
      }
      GCHandle gch = GCHandle.Alloc(gstId,GCHandleType.Pinned);
      GCHandle gch2 = GCHandle.Alloc(gstFloat,GCHandleType.Pinned);
      GCHandle gch3 = GCHandle.Alloc(gstTrackingId,GCHandleType.Pinned);
      int n = getContinuousGesture(kinect,gch.AddrOfPinnedObject(),gch2.AddrOfPinnedObject(),gch3.AddrOfPinnedObject());
      gch.Free();
      gch2.Free();
      gch3.Free();
      continuousGesture.Clear(); gestureProgress.Clear(); continuousGestureTrackingId.Clear();
      for (int i=0; i<n; i++) {
	continuousGesture.Add(gstId[i]);
	gestureProgress.Add(gstFloat[i]);
	continuousGestureTrackingId.Add(gstTrackingId[i]);
      }
      return n;
    }
    // Video
    public void openVideo(string filename) {
      IntPtr str = Marshal.StringToHGlobalUni(filename);
      openVideo(kinect,str);
      Marshal.FreeHGlobal(str);
    }
    public void writeVideo() { writeVideo(kinect); }
    public void closeVideo() { closeVideo(kinect); }
  }
  public class RigBone {
    public GameObject gameObject;
    public HumanBodyBones bone;
    public bool isValid;
    public Transform transform {
      get { return animator.GetBoneTransform(bone); }
    }
    Animator animator;
    Quaternion savedLocalRotation;
    Quaternion savedRotation;
    public RigBone(GameObject g, HumanBodyBones b) {
      gameObject = g;
      bone = b;
      isValid = false;
      animator = gameObject.GetComponent<Animator>();
      if (animator == null) {
	Debug.Log("no Animator Component");
	return;
      }
      Avatar avatar = animator.avatar;
      if (avatar == null || !avatar.isHuman || !avatar.isValid) {
	Debug.Log("Avatar is not Humanoid or it is not valid");
	return;
      }
      if (animator.GetBoneTransform(bone) == null) {
	Debug.Log("bone " + bone + " is note assigned in "+g);
	return;
      }
      isValid = true;
      savedLocalRotation = animator.GetBoneTransform(bone).localRotation;
      savedRotation = animator.GetBoneTransform(bone).rotation;
    }
    public void set(float a, float x, float y, float z) {
      set(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void set(Quaternion q) {
      animator.GetBoneTransform(bone).localRotation = q;
      savedLocalRotation = q;
    }
    public void mul(float a, float x, float y, float z) {
      mul(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void mul(Quaternion q) {
      Transform tr = animator.GetBoneTransform(bone);
      tr.localRotation = q * tr.localRotation;
    }
    public void offset(float a, float x, float y, float z) {
      offset(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void offset(Quaternion q) {
      animator.GetBoneTransform(bone).localRotation = q * savedLocalRotation;
    }
    public void gset(float a, float x, float y, float z) {
      gset(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void gset(Quaternion q) {
      animator.GetBoneTransform(bone).rotation = q;
      savedLocalRotation = q;
    }
    public void gmul(float a, float x, float y, float z) {
      gmul(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void gmul(Quaternion q) {
      Transform tr = animator.GetBoneTransform(bone);
      tr.rotation = q * tr.rotation;
    }
    public void goffset(float a, float x, float y, float z) {
      goffset(Quaternion.AngleAxis(a, new Vector3(x,y,z)));
    }
    public void goffset(Quaternion q) {
      animator.GetBoneTransform(bone).rotation = q * savedRotation;
    }
    public void changeBone(HumanBodyBones b) {
      bone = b;
      savedLocalRotation = animator.GetBoneTransform(bone).localRotation;
      savedRotation = animator.GetBoneTransform(bone).rotation;
    }
  }

  class HumanoidSkeleton {
    protected static int[] jointSegment = new int[] {
      Kinect.JointType_SpineBase, Kinect.JointType_SpineMid,             // Spine
      Kinect.JointType_Neck, Kinect.JointType_Head,                      // Neck
      // left
      Kinect.JointType_ShoulderLeft, Kinect.JointType_ElbowLeft,         // LeftUpperArm
      Kinect.JointType_ElbowLeft, Kinect.JointType_WristLeft,            // LeftLowerArm
      Kinect.JointType_WristLeft, Kinect.JointType_HandLeft,             // LeftHand
      Kinect.JointType_HipLeft, Kinect.JointType_KneeLeft,               // LeftUpperLeg
      Kinect.JointType_KneeLeft, Kinect.JointType_AnkleLeft,             // LeftLowerLeg6
      Kinect.JointType_AnkleLeft, Kinect.JointType_FootLeft,             // LeftFoot
      // right
      Kinect.JointType_ShoulderRight, Kinect.JointType_ElbowRight,       // RightUpperArm
      Kinect.JointType_ElbowRight, Kinect.JointType_WristRight,          // RightLowerArm
      Kinect.JointType_WristRight, Kinect.JointType_HandRight,           // RightHand
      Kinect.JointType_HipRight, Kinect.JointType_KneeRight,             // RightUpperLeg
      Kinect.JointType_KneeRight, Kinect.JointType_AnkleRight,           // RightLowerLeg
      Kinect.JointType_AnkleRight, Kinect.JointType_FootRight,           // RightFoot
    };
    public Vector3[] joint = new Vector3[Kinect.jointCount];
    public int[] jointState = new int[Kinect.jointCount];

    protected Dictionary<HumanBodyBones,Vector3> trackingSegment = null;
    protected Dictionary<HumanBodyBones, int>  trackingState = null;

    protected static HumanBodyBones[] humanBone = new HumanBodyBones[] {
      HumanBodyBones.Hips,
      HumanBodyBones.Spine,
      HumanBodyBones.Chest,
      HumanBodyBones.Neck,
      HumanBodyBones.Head,
      HumanBodyBones.LeftUpperArm,
      HumanBodyBones.LeftLowerArm,
      HumanBodyBones.LeftHand,
      HumanBodyBones.LeftUpperLeg,
      HumanBodyBones.LeftLowerLeg,
      HumanBodyBones.LeftFoot,
      HumanBodyBones.RightUpperArm,
      HumanBodyBones.RightLowerArm,
      HumanBodyBones.RightHand,
      HumanBodyBones.RightUpperLeg,
      HumanBodyBones.RightLowerLeg,
      HumanBodyBones.RightFoot,
    };

    protected static HumanBodyBones[] targetBone = new HumanBodyBones[] {
      HumanBodyBones.Spine,
      HumanBodyBones.Neck,
      HumanBodyBones.LeftUpperArm,
      HumanBodyBones.LeftLowerArm,
      HumanBodyBones.LeftHand,
      HumanBodyBones.LeftUpperLeg,
      HumanBodyBones.LeftLowerLeg,
      HumanBodyBones.LeftFoot,
      HumanBodyBones.RightUpperArm,
      HumanBodyBones.RightLowerArm,
      HumanBodyBones.RightHand,
      HumanBodyBones.RightUpperLeg,
      HumanBodyBones.RightLowerLeg,
      HumanBodyBones.RightFoot,
    };

    public GameObject humanoid;
    protected Dictionary<HumanBodyBones, RigBone> rigBone = null;
    protected bool isSavedPosition = false;
    protected Vector3 savedPosition;
    protected Quaternion savedHumanoidRotation;

    public HumanoidSkeleton(GameObject h) {
      humanoid = h;
      rigBone = new Dictionary<HumanBodyBones, RigBone>();
      foreach (HumanBodyBones bone in humanBone) {
	rigBone[bone] = new RigBone(humanoid,bone);
      }
      savedHumanoidRotation = humanoid.transform.rotation;
      trackingSegment = new Dictionary<HumanBodyBones,Vector3>(targetBone.Length);
      trackingState = new Dictionary<HumanBodyBones, int>(targetBone.Length);
    }
    protected void swapJoint(int a, int b) {
      Vector3 tmp = joint[a]; joint[a] = joint[b]; joint[b] = tmp;
      int t = jointState[a]; jointState[a] = jointState[b]; jointState[b] = t;
    }
    public void set(Kinect kinect, int n, bool mirrored = false, bool move=false, bool headMove=true) {
      Vector3 faceDir = kinect.getFaceDirectionByTrackingId(kinect.skeletonTrackingId[n]);
      if (isSavedPosition == false
	  && kinect.skeletonState[n][Kinect.JointType_SpineBase] != Kinect.TrackingState_NotTracked) {
	isSavedPosition = true;
	savedPosition = kinect.skeleton[n][Kinect.JointType_SpineBase];
      }
      for (int i=0; i<kinect.skeleton[n].Count; i++) {
	Vector3 jt = kinect.skeleton[n][i];
	if (mirrored) {
	  joint[i] = new Vector3(-jt.x, jt.y, -jt.z);
	} else {
	  joint[i] = new Vector3(jt.x, jt.y, savedPosition.z*2 - jt.z);
	}
	jointState[i] = kinect.skeletonState[n][i];
      }
      if (mirrored) {
	swapJoint(Kinect.JointType_ShoulderLeft, Kinect.JointType_ShoulderRight);
	swapJoint(Kinect.JointType_ElbowLeft, Kinect.JointType_ElbowRight);
	swapJoint(Kinect.JointType_WristLeft, Kinect.JointType_WristRight);
	swapJoint(Kinect.JointType_HandLeft, Kinect.JointType_HandRight);
	swapJoint(Kinect.JointType_HipLeft, Kinect.JointType_HipRight);
	swapJoint(Kinect.JointType_KneeLeft, Kinect.JointType_KneeRight);
	swapJoint(Kinect.JointType_AnkleLeft, Kinect.JointType_AnkleRight);
	swapJoint(Kinect.JointType_FootLeft, Kinect.JointType_FootRight);
	swapJoint(Kinect.JointType_HandTipLeft, Kinect.JointType_HandTipRight);
	swapJoint(Kinect.JointType_ThumbLeft, Kinect.JointType_ThumbRight);
      }
      for (int i=0; i<targetBone.Length; i++) {
	int s = jointSegment[2*i], e = jointSegment[2*i+1];
	trackingSegment[targetBone[i]] = joint[e] - joint[s];
	trackingState[targetBone[i]] = System.Math.Min(jointState[e],jointState[s]);
      }

      Vector3 waist = joint[Kinect.JointType_HipRight] - joint[Kinect.JointType_HipLeft];
      waist = new Vector3(waist.x, 0, waist.z);
      Quaternion rot = Quaternion.FromToRotation(Vector3.right,waist);
      Quaternion rotInv = Quaternion.Inverse(rot);
 
      Vector3 shoulder = joint[Kinect.JointType_ShoulderRight] - joint[Kinect.JointType_ShoulderLeft];
      shoulder = new Vector3(shoulder.x, 0, shoulder.z);
      //Quaternion srot = Quaternion.FromToRotation(Vector3.right,shoulder);
      //Quaternion srotInv = Quaternion.Inverse(srot);

      humanoid.transform.rotation = Quaternion.identity;
      //humanoid.transform.rotation = savedHumanoidRotation;
      foreach (HumanBodyBones bone in targetBone) {
	if (rigBone[bone].isValid && trackingState[bone] != Kinect.TrackingState_NotTracked) {
	  rigBone[bone].transform.rotation = rotInv * Quaternion.FromToRotation(Vector3.up,trackingSegment[bone]);
	}
      }
      //rigBone[HumanBodyBones.Chest].offset(srot);
      if (headMove && faceDir.magnitude > 1e-6) {
	float pitch = faceDir.x, yaw = faceDir.y, roll = faceDir.z;
	if (mirrored) {
	  pitch = -pitch;
	  roll = -roll;
	} else {
	  pitch = -pitch;
	  yaw = -yaw;
	}
	rigBone[HumanBodyBones.Head].transform.rotation = Util.toQ(pitch, yaw, roll);
      }
      Quaternion bodyRot = rot;
      if (mirrored) {
	bodyRot = Quaternion.AngleAxis(180,Vector3.up) * bodyRot;
      }
      humanoid.transform.rotation = bodyRot;
      if (move == true) {
	Vector3 m = joint[Kinect.JointType_SpineBase];
	if (mirrored) m = new Vector3(-m.x, m.y, -m.z);
	humanoid.transform.position = m;
      }
    }
  }
  class Util {
    public static Quaternion toQ (float pitch, float yaw, float roll) {
      yaw *= Mathf.Deg2Rad;
      pitch *= Mathf.Deg2Rad;
      roll *= Mathf.Deg2Rad;
      float rollOver2 = roll * 0.5f;
      float sinRollOver2 = (float)System.Math.Sin ((double)rollOver2);
      float cosRollOver2 = (float)System.Math.Cos ((double)rollOver2);
      float pitchOver2 = pitch * 0.5f;
      float sinPitchOver2 = (float)System.Math.Sin ((double)pitchOver2);
      float cosPitchOver2 = (float)System.Math.Cos ((double)pitchOver2);
      float yawOver2 = yaw * 0.5f;
      float sinYawOver2 = (float)System.Math.Sin ((double)yawOver2);
      float cosYawOver2 = (float)System.Math.Cos ((double)yawOver2);
      Quaternion result;
      result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
      result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
      result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
      result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
      return result;
    }
  }
  class UnityChanSkeleton: HumanoidSkeleton {
    new protected static int[] jointSegment = new int[] {
      // left
      Kinect.JointType_ShoulderLeft, Kinect.JointType_ElbowLeft,         // LeftUpperArm
      Kinect.JointType_ElbowLeft, Kinect.JointType_WristLeft,            // LeftLowerArm
      Kinect.JointType_WristLeft, Kinect.JointType_HandLeft,             // LeftHand
      Kinect.JointType_HipLeft, Kinect.JointType_KneeLeft,               // LeftUpperLeg
      Kinect.JointType_KneeLeft, Kinect.JointType_AnkleLeft,             // LeftLowerLeg6
      Kinect.JointType_AnkleLeft, Kinect.JointType_FootLeft,             // LeftFoot
      // right
      Kinect.JointType_ShoulderRight, Kinect.JointType_ElbowRight,       // RightUpperArm
      Kinect.JointType_ElbowRight, Kinect.JointType_WristRight,          // RightLowerArm
      Kinect.JointType_WristRight, Kinect.JointType_HandRight,           // RightHand
      Kinect.JointType_HipRight, Kinect.JointType_KneeRight,             // RightUpperLeg
      Kinect.JointType_KneeRight, Kinect.JointType_AnkleRight,           // RightLowerLeg
      Kinect.JointType_AnkleRight, Kinect.JointType_FootRight,           // RightFoot
    };
    new protected static HumanBodyBones[] targetBone = new HumanBodyBones[] {
      HumanBodyBones.LeftUpperArm,
      HumanBodyBones.LeftLowerArm,
      HumanBodyBones.LeftHand,
      HumanBodyBones.LeftUpperLeg,
      HumanBodyBones.LeftLowerLeg,
      HumanBodyBones.LeftFoot,
      HumanBodyBones.RightUpperArm,
      HumanBodyBones.RightLowerArm,
      HumanBodyBones.RightHand,
      HumanBodyBones.RightUpperLeg,
      HumanBodyBones.RightLowerLeg,
      HumanBodyBones.RightFoot,
    };
    public UnityChanSkeleton(GameObject h):base(h) {}
    new public void set(Kinect kinect, int n, bool mirrored=false, bool move=false, bool headMove=false) {
      Vector3 faceDir = kinect.getFaceDirectionByTrackingId(kinect.skeletonTrackingId[n]);
      if (isSavedPosition == false
	  && kinect.skeletonState[n][Kinect.JointType_SpineBase] != Kinect.TrackingState_NotTracked) {
	isSavedPosition = true;
	savedPosition = kinect.skeleton[n][Kinect.JointType_SpineBase];
      }
      for (int i=0; i<kinect.skeleton[n].Count; i++) {
	Vector3 jt = kinect.skeleton[n][i];
	if (mirrored) {
	  joint[i] = new Vector3(-jt.x, jt.y, -jt.z);
	} else {
	  joint[i] = new Vector3(jt.x, jt.y, savedPosition.z*2 - jt.z);
	}
	jointState[i] = kinect.skeletonState[n][i];
      }
      if (mirrored) {
	swapJoint(Kinect.JointType_ShoulderLeft, Kinect.JointType_ShoulderRight);
	swapJoint(Kinect.JointType_ElbowLeft, Kinect.JointType_ElbowRight);
	swapJoint(Kinect.JointType_WristLeft, Kinect.JointType_WristRight);
	swapJoint(Kinect.JointType_HandLeft, Kinect.JointType_HandRight);
	swapJoint(Kinect.JointType_HipLeft, Kinect.JointType_HipRight);
	swapJoint(Kinect.JointType_KneeLeft, Kinect.JointType_KneeRight);
	swapJoint(Kinect.JointType_AnkleLeft, Kinect.JointType_AnkleRight);
	swapJoint(Kinect.JointType_FootLeft, Kinect.JointType_FootRight);
	swapJoint(Kinect.JointType_HandTipLeft, Kinect.JointType_HandTipRight);
	swapJoint(Kinect.JointType_ThumbLeft, Kinect.JointType_ThumbRight);
      }
      for (int i=0; i<targetBone.Length; i++) {
	int s = jointSegment[2*i], e = jointSegment[2*i+1];
	trackingSegment[targetBone[i]] = joint[e] - joint[s];
	trackingState[targetBone[i]] = System.Math.Min(jointState[e],jointState[s]);
      }

      savedHumanoidRotation = humanoid.transform.rotation;
      humanoid.transform.rotation = Quaternion.identity;

      Vector3 waist = joint[Kinect.JointType_HipRight] - joint[Kinect.JointType_HipLeft];
      waist = new Vector3(waist.x, 0, waist.z);
      Quaternion rot = Quaternion.FromToRotation(Vector3.right,waist);
      Quaternion rotInv = Quaternion.Inverse(rot);

      Vector3 shoulder = joint[Kinect.JointType_ShoulderRight] - joint[Kinect.JointType_ShoulderLeft];
      shoulder = new Vector3(shoulder.x, 0, shoulder.z);
      Quaternion srot = Quaternion.FromToRotation(Vector3.right,shoulder);
      
      Quaternion defaultQ = Quaternion.AngleAxis(90, new Vector3(0,1,0) )
	* Quaternion.AngleAxis( -90, new Vector3(0,0,1 ) );
      foreach (HumanBodyBones b in targetBone) {
	if (rigBone[b].isValid && trackingState[b] != Kinect.TrackingState_NotTracked) {
	  rigBone[b].transform.rotation = rotInv * Quaternion.FromToRotation(Vector3.up,trackingSegment[b]) * defaultQ;
	}
      }

      Quaternion q = Quaternion.AngleAxis(-90, new Vector3(0,1,0))
	* Quaternion.AngleAxis(-90, new Vector3(0,0,1));
      if (headMove && faceDir.magnitude > 1e-6) {
	float pitch = faceDir.x, yaw = faceDir.y, roll = faceDir.z;
	if (mirrored) {
	  pitch = -pitch;
	  roll = -roll;
	} else {
	  pitch = -pitch;
	  yaw = -yaw;
	}
	rigBone[HumanBodyBones.Head].transform.rotation = Util.toQ(pitch, yaw, roll) * q;
      }

      if (rigBone[HumanBodyBones.Chest].isValid)
	rigBone[HumanBodyBones.Chest].transform.rotation = srot * q;
      
      if (mirrored) {
	humanoid.transform.rotation = Quaternion.AngleAxis(180,Vector3.up) * rot;
      } else {
	humanoid.transform.rotation = rot;
      }
      if (move == true) {
	Vector3 m = joint[Kinect.JointType_SpineBase];
	if (mirrored) m = new Vector3(-m.x, m.y, -m.z);
	humanoid.transform.position = m;
      }
      return;
    }
  }
}
