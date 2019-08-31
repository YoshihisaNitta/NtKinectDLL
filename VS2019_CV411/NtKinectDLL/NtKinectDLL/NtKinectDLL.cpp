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
 *    NtKinect version 1.8.2 and after
 */

#include "pch.h"
#include "framework.h"

#include <unordered_map>
#include "NtKinectDLL.h"

#define USE_AUDIO
#define USE_FACE
#define USE_GESTURE
#define USE_THREAD
#include "NtKinect.h"

using namespace std;

namespace NtKinectDLL {
  string wchar2string(wchar_t* name) {
    int len = WideCharToMultiByte(CP_UTF8, NULL, name, -1, NULL, 0, NULL, NULL) + 1;
    char* nameBuffer = new char[len];
    memset(nameBuffer, '\0', len);
    WideCharToMultiByte(CP_UTF8, NULL, name, -1, nameBuffer, len, NULL, NULL);
    string s(nameBuffer);
    return s;
  }

  NTKINECTDLL_API void* getKinect(void) {
    NtKinect* kinect = new NtKinect();
    return static_cast<void*>(kinect);
  }
  NTKINECTDLL_API void stopKinect(void* ptr) {
    cv::destroyAllWindows();
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    delete kinect;
  }

  // OpenCV
  NTKINECTDLL_API void imshow(void* ptr) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    int scale = 4;
    cv::Mat img((*kinect).rgbImage);
    cv::resize(img, img, cv::Size(img.cols / scale, img.rows / scale), 0, 0);
    for (auto& person : (*kinect).skeleton) {
      for (auto& joint : person) {
	if (joint.TrackingState == TrackingState_NotTracked) continue;
	ColorSpacePoint cp;
	(*kinect).coordinateMapper->MapCameraPointToColorSpace(joint.Position, &cp);
	cv::rectangle(img, cv::Rect((int)cp.X / scale - 2, (int)cp.Y / scale - 2, 4, 4), cv::Scalar(0, 0, 255), 2);
      }
    }
    for (auto r : (*kinect).faceRect) {
      cv::Rect r2(r.x / scale, r.y / scale, r.width / scale, r.height / scale);
      cv::rectangle(img, r2, cv::Scalar(255, 255, 0), 2);
    }
    cv::imshow("face", img);
    cv::waitKey(1);
  }

  vector<cv::Rect> savedRect;
  NTKINECTDLL_API void imshowBlack(void* ptr) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    int scale = 4;
    cv::Mat img((*kinect).rgbImage);
    cv::resize(img, img, cv::Size(img.cols / scale, img.rows / scale), 0, 0);
    if ((*kinect).faceRect.size() == 0) {
      for (auto& r : savedRect) {
	(*kinect).faceRect.push_back(r);
      }
    }
    else {
      savedRect.clear();
      for (auto& r : (*kinect).faceRect) {
	savedRect.push_back(r);
      }
    }
    for (auto r : (*kinect).faceRect) {
      cv::Rect r2(r.x / scale, r.y / scale, r.width / scale, r.height / scale);
      cv::rectangle(img, r2, cv::Scalar(0, 0, 0), -1);
    }
    for (auto& person : (*kinect).skeleton) {
      for (auto& joint : person) {
	if (joint.TrackingState == TrackingState_NotTracked) continue;
	ColorSpacePoint cp;
	(*kinect).coordinateMapper->MapCameraPointToColorSpace(joint.Position, &cp);
	cv::rectangle(img, cv::Rect((int)cp.X / scale - 2, (int)cp.Y / scale - 2, 4, 4), cv::Scalar(0, 0, 255), 2);
      }
    }
    for (auto r : (*kinect).faceRect) {
      cv::Rect r2(r.x / scale, r.y / scale, r.width / scale, r.height / scale);
      cv::rectangle(img, r2, cv::Scalar(255, 255, 0), 2);
    }
    cv::imshow("face", img);
    cv::waitKey(1);
  }

  // CoordinateMapper
  /*
    NTKINECTDLL_API void mapCameraPointToColorSpace(void* ptr,void* sv,void* cv) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    CameraSpacePoint sp; sp.X = ((float*)sv)[0]; sp.Y = ((float*)sv)[1]; sp.Z = ((float*)sv)[2];
    ColorSpacePoint cp;
    (*kinect).coordinateMapper->MapCameraPointToColorSpace(sp,&cp);
    ((float*)cv)[0] = cp.X; ((float*)cv)[1] = cp.Y;
    }
    NTKINECTDLL_API void mapCameraPointToDepthSpace(void* ptr,void* sv,void* dv) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    CameraSpacePoint sp; sp.X = ((float*)sv)[0]; sp.Y = ((float*)sv)[1]; sp.Z = ((float*)sv)[2];
    DepthSpacePoint dp;
    (*kinect).coordinateMapper->MapCameraPointToDepthSpace(sp,&dp);
    ((float*)dv)[0] = dp.X; ((float*)dv)[1] = dp.Y;
    }
    NTKINECTDLL_API void mapDepthPointToColorSpace(void* ptr,void* dv,UINT16 depth,void* cv) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    DepthSpacePoint dp; dp.X = ((float*)dv)[0]; dp.Y = ((float*)dv)[1];
    ColorSpacePoint cp;
    (*kinect).coordinateMapper->MapDepthPointToColorSpace(dp,depth,&cp);
    ((float*)cv)[0] = cp.X; ((float*)cv)[1] = cp.Y;
    }
    NTKINECTDLL_API void mapDepthPointToCameraSpace(void* ptr,void* dv,UINT16 depth,void* sv) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    DepthSpacePoint dp; dp.X = ((float*)dv)[0]; dp.Y = ((float*)dv)[1];
    CameraSpacePoint sp;
    (*kinect).coordinateMapper->MapDepthPointToCameraSpace(dp,depth,&sp);
    ((float*)sv)[0] = sp.X; ((float*)sv)[1] = sp.Y; ((float*)sv)[2] = sp.Z;
    }
  */
  NTKINECTDLL_API void mapCameraPointToColorSpace(void* ptr, void* sv, void* cv, int n) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* sa = (float*)sv;
    float* ca = (float*)cv;
    for (int i = 0; i<n; i++) {
      CameraSpacePoint sp; sp.X = *sa++; sp.Y = *sa++; sp.Z = *sa++;
      ColorSpacePoint cp;
      (*kinect).coordinateMapper->MapCameraPointToColorSpace(sp, &cp);
      *ca++ = cp.X; *ca++ = cp.Y;
    }
  }
  NTKINECTDLL_API void mapCameraPointToDepthSpace(void* ptr, void* sv, void* dv, int n) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* sa = (float*)sv;
    float* da = (float*)dv;
    for (int i = 0; i<n; i++) {
      CameraSpacePoint sp; sp.X = *sa++; sp.Y = *sa++; sp.Z = *sa++;
      DepthSpacePoint dp;
      (*kinect).coordinateMapper->MapCameraPointToDepthSpace(sp, &dp);
      *da++ = dp.X; *da++ = dp.Y;
    }
  }
  NTKINECTDLL_API void mapDepthPointToColorSpace(void* ptr, void* dv, void* dth, void* cv, int n) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* da = (float*)dv;
    UINT16* dth_addr = (UINT16*)dth;
    float* ca = (float*)cv;
    for (int i = 0; i<n; i++) {
      DepthSpacePoint dp; dp.X = *da++; dp.Y = *da++;
      ColorSpacePoint cp;
      (*kinect).coordinateMapper->MapDepthPointToColorSpace(dp, *dth_addr++, &cp);
      *ca++ = cp.X; *ca++ = cp.Y;
    }
  }
  NTKINECTDLL_API void mapDepthPointToCameraSpace(void* ptr, void* dv, void* dth, void* sv, int n) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* da = (float*)dv;
    UINT16* dth_addr = (UINT16*)dth;
    float* sa = (float*)sv;
    for (int i = 0; i<n; i++) {
      DepthSpacePoint dp; dp.X = *da++; dp.Y = *da++;
      CameraSpacePoint sp;
      (*kinect).coordinateMapper->MapDepthPointToCameraSpace(dp, *dth_addr++, &sp);
      *sa++ = sp.X; *sa++ = sp.Y; *sa++ = sp.Z;
    }
  }

  // Multi Thread
  NTKINECTDLL_API void acquire(void* ptr) { (*static_cast<NtKinect*>(ptr)).acquire(); }
  NTKINECTDLL_API void release(void* ptr) { (*static_cast<NtKinect*>(ptr)).release(); }

  // Audio
  NTKINECTDLL_API void setAudio(void* ptr, bool flag) { (*static_cast<NtKinect*>(ptr)).setAudio(flag); }
  NTKINECTDLL_API float getBeamAngle(void* ptr) { return (*static_cast<NtKinect*>(ptr)).beamAngle; }
  NTKINECTDLL_API float getBeamAngleConfidence(void* ptr) { return (*static_cast<NtKinect*>(ptr)).beamAngleConfidence; }
  NTKINECTDLL_API unsigned __int64 getAudioTrackingId(void* ptr) { return (*static_cast<NtKinect*>(ptr)).audioTrackingId; }
  NTKINECTDLL_API void openAudio(void* ptr, wchar_t* filename) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    (*kinect).openAudio(wchar2string(filename));
  }
  NTKINECTDLL_API void closeAudio(void* ptr) { (*static_cast<NtKinect*>(ptr)).closeAudio(); }
  NTKINECTDLL_API bool isOpenedAudio(void* ptr) { return (*static_cast<NtKinect*>(ptr)).isOpenedAudio(); }

  // RGB
  NTKINECTDLL_API void setRGB(void* ptr) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    (*kinect).setRGB();
  }
  NTKINECTDLL_API int getRGB(void* ptr, void* data) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    char* idx = (char*)data;
    for (int y = 0; y<(*kinect).rgbImage.rows; y++) {
      for (int x = 0; x<(*kinect).rgbImage.cols; x++) {
	cv::Vec4b& pxl = (*kinect).rgbImage.at<cv::Vec4b>(y, x);
	*idx++ = pxl[2]; // Red
	*idx++ = pxl[1]; // Green
	*idx++ = pxl[0]; // Blue
	*idx++ = pxl[3]; // Alpha
      }
    }
    return (int)(idx - (char*)data);
  }

  // Depth
  NTKINECTDLL_API void setDepth(void* ptr) { (*static_cast<NtKinect*>(ptr)).setDepth(); }
  NTKINECTDLL_API int getDepth(void* ptr, void* data) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    UINT16* idx = (UINT16*)data;
    for (int y = 0; y<(*kinect).depthImage.rows; y++) {
      for (int x = 0; x<(*kinect).depthImage.cols; x++) {
	*idx++ = (*kinect).depthImage.at<UINT16>(y, x);
      }
    }
    return (int)(idx - (UINT16*)data);
  }

  // Infrared
  NTKINECTDLL_API void setInfrared(void* ptr) { (*static_cast<NtKinect*>(ptr)).setInfrared(); }
  NTKINECTDLL_API int getInfrared(void* ptr, void* data) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    UINT16* idx = (UINT16*)data;
    for (int y = 0; y<(*kinect).infraredImage.rows; y++) {
      for (int x = 0; x<(*kinect).infraredImage.cols; x++) {
	*idx++ = (*kinect).infraredImage.at<UINT16>(y, x);
      }
    }
    return (int)(idx - (UINT16*)data);
  }

  // bodyIndex
  NTKINECTDLL_API void setBodyIndex(void* ptr) { (*static_cast<NtKinect*>(ptr)).setBodyIndex(); }
  NTKINECTDLL_API int getBodyIndex(void* ptr, void* data) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    char* idx = (char*)data;
    for (int y = 0; y<(*kinect).bodyIndexImage.rows; y++) {
      for (int x = 0; x<(*kinect).bodyIndexImage.cols; x++) {
	*idx++ = (*kinect).bodyIndexImage.at<char>(y, x);
      }
    }
    return (int)(idx - (char*)data);
  }

  // Skeleton
  NTKINECTDLL_API void setSkeleton(void* ptr) { (*static_cast<NtKinect*>(ptr)).setSkeleton(); }
  NTKINECTDLL_API int getSkeleton(void* ptr, void* skel, void* skelState, void* skelId, void* skelTrackingId) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* skeleton = (float*)skel;
    int* state = (int*)skelState;
    int* id = (int*)skelId;
    UINT64* trackingId = (UINT64*)skelTrackingId;
    int idx = 0, jt = 0, st = 0;
    for (auto& person : (*kinect).skeleton) {
      for (auto& joint : person) {
	skeleton[jt++] = joint.Position.X;
	skeleton[jt++] = joint.Position.Y;
	skeleton[jt++] = joint.Position.Z;
	state[st++] = joint.TrackingState;
      }
      id[idx] = (*kinect).skeletonId[idx];
      trackingId[idx] = (*kinect).skeletonTrackingId[idx];
      idx++;
    }
    return idx;
  }
  NTKINECTDLL_API int handState(void* ptr, int id, bool isLeft) { return (*static_cast<NtKinect*>(ptr)).handState(id, isLeft).first; }

  // Face
  NTKINECTDLL_API void setFace(void* ptr, bool isColorSpace) { (*static_cast<NtKinect*>(ptr)).setFace(isColorSpace); }
  NTKINECTDLL_API int getFace(void* ptr, float* point, float* rect, float* direction, int* property, void* tid) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float* p = point;
    for (auto& face : (*kinect).facePoint) {
      for (auto& pt : face) {
	*p++ = pt.X;
	*p++ = pt.Y;
      }
    }
    int np = (int)(p - point) / 2;
    p = rect;
    for (auto& r : (*kinect).faceRect) {
      *p++ = (float)r.x;
      *p++ = (float)r.y;
      *p++ = (float)r.width;
      *p++ = (float)r.height;
    }
    int nr = (int)(p - rect) / 4;
    p = direction;
    for (auto& d : (*kinect).faceDirection) {
      *p++ = d[0];
      *p++ = d[1];
      *p++ = d[2];
    }
    int nd = (int)(p - direction) / 3;
    int* a = (int*)property;
    for (auto& face : (*kinect).faceProperty) {
      for (auto& prop : face) {
	*a++ = prop;
      }
    }
    int npr = (int)(a - property);
    UINT64* q = (UINT64*)tid;
    for (auto& t : (*kinect).faceTrackingId) {
      *q++ = t;
    }
    int nt = (int)(q - (UINT64*)tid);
    return min(nt, min(min(npr, nd), min(nr, np)));
  }

  // HDFace
  NTKINECTDLL_API void setHDFace(void* ptr) { (*static_cast<NtKinect*>(ptr)).setHDFace(); }
  NTKINECTDLL_API int getHDFace(void* ptr, float* point, void* tid, int* status) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    float *p = (float*)point;
    for (auto& person : (*kinect).hdfaceVertices) {
      for (auto& cp : person) {
	*p++ = cp.X;
	*p++ = cp.Y;
	*p++ = cp.Z;
      }
    }
    UINT64 *q = (UINT64*)tid;
    for (auto& t : (*kinect).hdfaceTrackingId) {
      *q++ = t;
    }
    int* r = (int*)status;
    for (auto& s : (*kinect).hdfaceStatus) {
      *r++ = s.first;
      *r++ = s.second;
    }
    return (int)(*kinect).hdfaceVertices.size();
  }

  // Gesture
  NTKINECTDLL_API void setGestureFile(void* ptr, wchar_t* filename) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    wstring fname(filename);
    (*kinect).setGestureFile(fname);
  }

  NTKINECTDLL_API int setGestureId(void* ptr, wchar_t* name, int id) {
    int len = WideCharToMultiByte(CP_UTF8, NULL, name, -1, NULL, 0, NULL, NULL) + 1;
    char* nameBuffer = new char[len];
    memset(nameBuffer, '\0', len);
    WideCharToMultiByte(CP_UTF8, NULL, name, -1, nameBuffer, len, NULL, NULL);
    string s(nameBuffer);
    gidMap[s] = id;

    return id;
  }

  NTKINECTDLL_API void setGesture(void* ptr) { (*static_cast<NtKinect*>(ptr)).setGesture(); }

  NTKINECTDLL_API int getDiscreteGesture(void* ptr, int* gid, float* confidence, void *tid) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    UINT64* trackingId = (UINT64*)tid;
    for (int i = 0; i<(*kinect).discreteGesture.size(); i++) {
      auto g = (*kinect).discreteGesture[i];
      string gname = (*kinect).gesture2string(g.first);
      gid[i] = gidMap[gname];
      confidence[i] = g.second;
      trackingId[i] = (*kinect).discreteGestureTrackingId[i];
    }
    return (int)(*kinect).discreteGesture.size();
  }

  NTKINECTDLL_API int getContinuousGesture(void* ptr, int* gid, float* progress, void *tid) {
    NtKinect* kinect = static_cast<NtKinect*>(ptr);
    UINT64* trackingId = (UINT64*)tid;
    for (int i = 0; i<(*kinect).continuousGesture.size(); i++) {
      auto g = (*kinect).continuousGesture[i];
      string gname = (*kinect).gesture2string(g.first);
      gid[i] = gidMap[gname];
      progress[i] = g.second;
      trackingId[i] = (*kinect).continuousGestureTrackingId[i];
    }
    return (int)(*kinect).continuousGesture.size();
  }
  NTKINECTDLL_API int getGidMapSize() {
    return (int)gidMap.size();
  }

  // Video
  cv::VideoWriter *videoWriter = nullptr;
  cv::Size videoSize;
  bool videoOnSave = false;

  NTKINECTDLL_API void openVideo(void* ptr, wchar_t* filename) {
    NtKinect *kinect = static_cast<NtKinect*>(ptr);
    string fname = wchar2string(filename);
    if (videoOnSave) {
      std::cerr << "cannot open two video files simultaneously" << std::endl;
      return;
    }
    videoSize = cv::Size(1920 / 4, 1080 / 4);
    videoWriter = new cv::VideoWriter(fname, cv::VideoWriter::fourcc('X', 'V', 'I', 'D'), 30.0, videoSize);
    if (!(*videoWriter).isOpened()) {
      std::cerr << "cannot open video file" << std::endl;
      return;
    }
    videoOnSave = true;
  }
  NTKINECTDLL_API void writeVideo(void* ptr) {
    NtKinect *kinect = static_cast<NtKinect*>(ptr);
    cv::Mat img;
    if (videoOnSave) {
      cv::resize((*kinect).rgbImage, img, videoSize, 0, 0);
      cv::cvtColor(img, img, cv::COLOR_BGRA2BGR);
      (*videoWriter) << img;
    }
  }
  NTKINECTDLL_API void closeVideo(void* ptr) {
    if (videoOnSave) {
      (*videoWriter).release();
      delete videoWriter;
      videoWriter = nullptr;
      videoOnSave = false;
    }
  }
}
