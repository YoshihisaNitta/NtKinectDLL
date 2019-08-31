/*
 * Copyright (c) 2017 Yoshihisa Nitta
 * Released under the MIT license
 * http://opensource.org/licenses/mit-license.php
 */

/*
 * NtKinectDLL.h version 1.2.6: 2017/11/08
 * http://nw.tsuda.ac.jp/lec/kinect2/NtKinectDLL
 *
 * requires:
 *    NtKinect version 1.8.2 and later
 */

#ifdef NTKINECTDLL_EXPORTS
#define NTKINECTDLL_API __declspec(dllexport)
#else
#define NTKINECTDLL_API __declspec(dllimport)
#endif

namespace NtKinectDLL {
  extern "C" {
    NTKINECTDLL_API void* getKinect(void);
    NTKINECTDLL_API void stopKinect(void* ptr);

    // OpenCV
    NTKINECTDLL_API void imshow(void* ptr);
    NTKINECTDLL_API void imshowBlack(void* ptr);

    // CoordinateMapper
    /*
      NTKINECTDLL_API void mapCameraPointToColorSpace(void* ptr,void* sv,void* cv);
      NTKINECTDLL_API void mapCameraPointToDepthSpace(void* ptr,void* sv,void* dv);
      NTKINECTDLL_API void mapDepthPointToColorSpace(void* ptr,void* dv,UINT16 depth,void* cv);
      NTKINECTDLL_API void mapDepthPointToCameraSpace(void* ptr,void* dv,UINT16 depth,void* sv);
    */
    NTKINECTDLL_API void mapCameraPointToColorSpace(void* ptr, void* sv, void* cv, int n);
    NTKINECTDLL_API void mapCameraPointToDepthSpace(void* ptr, void* sv, void* dv, int n);
    NTKINECTDLL_API void mapDepthPointToColorSpace(void* ptr, void* dv, void* dth, void* cv, int n);
    NTKINECTDLL_API void mapDepthPointToCameraSpace(void* ptr, void* dv, void* dth, void* sv, int n);

    // Multi Thread
    NTKINECTDLL_API void acquire(void* ptr);
    NTKINECTDLL_API void release(void* ptr);

    // Audio
    NTKINECTDLL_API void setAudio(void* ptr, bool flag);
    NTKINECTDLL_API float getBeamAngle(void* ptr);
    NTKINECTDLL_API float getBeamAngleConfidence(void* ptr);
    NTKINECTDLL_API unsigned __int64 getAudioTrackingId(void* ptr);
    NTKINECTDLL_API void openAudio(void* ptr, wchar_t* filename);
    NTKINECTDLL_API void closeAudio(void* ptr);
    NTKINECTDLL_API bool isOpenedAudio(void* ptr);

    // RGB
    NTKINECTDLL_API void setRGB(void* ptr);
    NTKINECTDLL_API int getRGB(void* ptr, void* data);

    // Depth
    NTKINECTDLL_API void setDepth(void* ptr);
    NTKINECTDLL_API int getDepth(void* ptr, void* data);

    // Infrared
    NTKINECTDLL_API void setInfrared(void* ptr);
    NTKINECTDLL_API int getInfrared(void* ptr, void* data);

    // BodyIndex
    NTKINECTDLL_API void setBodyIndex(void* ptr);
    NTKINECTDLL_API int getBodyIndex(void* ptr, void* data);

    // Skeleton
    NTKINECTDLL_API void setSkeleton(void* ptr);
    NTKINECTDLL_API int getSkeleton(void* ptr, void* skelton, void* state, void* id, void* tid);
    NTKINECTDLL_API int handState(void* ptr, int id, bool isLeft);

    // Face
    NTKINECTDLL_API void setFace(void* ptr, bool flag);
    NTKINECTDLL_API int getFace(void* ptr, float* point, float* rect, float* direction, int* property, void* tid);

    // HDFace
    NTKINECTDLL_API void setHDFace(void* ptr);
    NTKINECTDLL_API int getHDFace(void* ptr, float* point, void* tid, int *status);

    // Gesture
    NTKINECTDLL_API void setGestureFile(void* ptr, wchar_t* filename);
    NTKINECTDLL_API int setGestureId(void* ptr, wchar_t* name, int id); // id: non-zero
    NTKINECTDLL_API void setGesture(void* ptr);
    NTKINECTDLL_API int getDiscreteGesture(void* ptr, int* gid, float* confidence, void* tid);
    NTKINECTDLL_API int getContinuousGesture(void* ptr, int* gid, float* progress, void* tid);
    NTKINECTDLL_API int getGidMapSize();

    // Video
    NTKINECTDLL_API void openVideo(void* ptr, wchar_t* filename);
    NTKINECTDLL_API void writeVideo(void* ptr);
    NTKINECTDLL_API void closeVideo(void* ptr);
  }

  //Gesture
  std::unordered_map<std::string, int> gidMap;
}
