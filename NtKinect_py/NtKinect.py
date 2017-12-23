# ################################################
# Copyright (c) 2017 by Yoshihisa Nitta
# Released under the MIT License
# http://opensource.org/licenses/mit-license.php
# ################################################

# NtKinect.py version 0.1 2017/11/08
#  http://nw.tsuda.ac.jp/lec/NtKinectDLL/
#
# requires:
#  NtKinectDLL version 1.2.5 or later

from ctypes import *

# ##############
# Constants
# ##############
# Number
bodyCount = 6
jointCount = 25
rgbCols = 1920
rgbRows = 1080
depthCols = 512
depthRows = 424
# JointType
JointType_SpineBase= 0
JointType_SpineMid= 1
JointType_Neck= 2
JointType_Head= 3
JointType_ShoulderLeft= 4
JointType_ElbowLeft= 5
JointType_WristLeft= 6
JointType_HandLeft= 7
JointType_ShoulderRight= 8
JointType_ElbowRight= 9
JointType_WristRight= 10
JointType_HandRight= 11
JointType_HipLeft= 12
JointType_KneeLeft= 13
JointType_AnkleLeft= 14
JointType_FootLeft= 15
JointType_HipRight= 16
JointType_KneeRight= 17
JointType_AnkleRight= 18
JointType_FootRight= 19
JointType_SpineShoulder= 20
JointType_HandTipLeft= 21
JointType_ThumbLeft= 22
JointType_HandTipRight= 23
JointType_ThumbRight= 24
# TrackingState
TrackingState_NotTracked= 0
TrackingState_Inferred= 1
TrackingState_Tracked= 2
# FacePoint
FacePointType_None= -1
FacePointType_EyeLeft= 0
FacePointType_EyeRight= 1
FacePointType_Nose= 2
FacePointType_MouthCornerLeft= 3
FacePointType_MouthCornerRight= 4
FacePointType_Count= FacePointType_MouthCornerRight + 1
# a_FaceProperty
FaceProperty_Happy= 0
FaceProperty_Engaged= 1
FaceProperty_WearingGlasses= 2
FaceProperty_LeftEyeClosed= 3
FaceProperty_RightEyeClosed= 4
FaceProperty_MouthOpen= 5
FaceProperty_MouthMoved= 6
FaceProperty_LookingAway= 7
FaceProperty_Count= FaceProperty_LookingAway + 1
# FaceDetectionResult
DetectionResult_Unknown= 0
DetectionResult_No= 1
DetectionResult_Maybe= 2
DetectionResult_Yes= 3
# HDFace
HDFaceVerticesSize = 1347

# ##################
# DLL Functions
# ################
nt=windll.LoadLibrary('NtKinectDLL.dll')
nt.getKinect.argtypes = (None)
nt.getKinect.restype = c_void_p
nt.stopKinect.argtypes=[c_void_p]
nt.stopKinect.restype = None
# OpenCV
nt.imshow.argtypes=[c_void_p]
nt.imshow.restype=None
nt.imshowBlack.argtypes=[c_void_p]
nt.imshowBlack.restype=None
# CoordinateMapper
nt.mapCameraPointToColorSpace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_float),c_int]
nt.mapCameraPointToColorSpace.restype=None
nt.mapCameraPointToDepthSpace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_float),c_int]
nt.mapCameraPointToDepthSpace.restype=None
nt.mapDepthPointToColorSpace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_ushort),POINTER(c_float),c_int]
nt.mapDepthPointToColorSpace.restype=None
nt.mapDepthPointToCameraSpace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_ushort),POINTER(c_float),c_int]
nt.mapDepthPointToCameraSpace.restype=None
# Multi Thread
nt.acquire.argtypes=[c_void_p]
nt.acquire.restype=None
nt.release.argtypes=[c_void_p]
nt.release.restype=None
# Audio
nt.setAudio.argtypes=[c_bool]
nt.setAudio.restype=None
nt.getBeamAngle.argtypes=[c_void_p]
nt.getBeamAngle.restype=c_float
nt.getBeamAngleConfidence.argtypes=[c_void_p]
nt.getBeamAngleConfidence.restype=c_float
nt.openAudio.argtypes=[c_void_p,c_char_p]
nt.openAudio.restype=None
nt.closeAudio.argtypes=[c_void_p]
nt.closeAudio.restype=None
nt.isOpenedAudio.argtypes=[c_void_p]
nt.isOpenedAudio.restype=c_bool
# RGB
nt.setRGB.argtypes=[c_void_p]
nt.setRGB.restype=None
nt.getRGB.argtypes=[c_void_p,POINTER(c_ubyte)]
nt.getRGB.restype=c_int
# Depth
nt.setDepth.argtypes=[c_void_p]
nt.setDepth.restype=None
nt.getDepth.argtypes=[c_void_p,POINTER(c_ushort)]
nt.getDepth.restype=c_int
# Infrared
nt.setInfrared.argtypes=[c_void_p]
nt.setInfrared.restype=None
nt.getInfrared.argtypes=[c_void_p,POINTER(c_ushort)]
nt.getInfrared.restype=c_int
# BodyIndex
nt.setBodyIndex.argtypes=[c_void_p]
nt.setBodyIndex.restype=None
nt.getBodyIndex.argtypes=[c_void_p,POINTER(c_ubyte)]
nt.getBodyIndex.restype=c_int
# Skeleton
nt.setSkeleton.argtypes=[c_void_p]
nt.setSkeleton.restype=None
nt.getSkeleton.argtypes=[c_void_p,POINTER(c_float),POINTER(c_int),POINTER(c_int),POINTER(c_uint64)]
nt.getSkeleton.restype=c_int
nt.handState.argtypes=[c_void_p,c_int,c_bool]
nt.handState.restype=c_int
# Face
nt.setFace.argtypes=[c_void_p,c_bool]
nt.setFace.restype=None
nt.getFace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_float),POINTER(c_float),POINTER(c_int),POINTER(c_uint64)]
nt.getFace.restype=c_int
# HDFace
nt.setHDFace.argtypes=[c_void_p]
nt.setHDFace.restype=None
nt.getHDFace.argtypes=[c_void_p,POINTER(c_float),POINTER(c_uint64),POINTER(c_int)]
nt.getHDFace.restype=c_int
# Gesture
nt.setGestureFile.argtypes=[c_void_p,c_wchar_p]
nt.setGestureFile.restype=None
nt.setGesture.argtypes=[c_void_p]
nt.setGesture.restype=None
nt.getDiscreteGesture.argtypes=[c_void_p,POINTER(c_int),POINTER(c_float),POINTER(c_uint64)]
nt.getDiscreteGesture.restype=c_int
nt.getContinuousGesture.argtypes=[c_void_p,POINTER(c_int),POINTER(c_float),POINTER(c_uint64)]
nt.getContinuousGesture.restype=c_int
nt.getGidMapSize.argtypes=(None)
nt.getGidMapSize.restype=c_int
# Video
nt.openVideo.argtypes=[c_void_p,c_wchar_p]
nt.openVideo.restype=None
nt.writeVideo.argtypes=[c_void_p]
nt.writeVideo.restype=None
nt.closeVideo.argtypes=[c_void_p]
nt.closeVideo.restype=None

# ###################
# Python Variables
# ###################
kinect = nt.getKinect()

beamAngle = c_float(0.0)
beamAngleConfidence = c_float(0.0)
audioTrackingId = c_uint64(0)

rgbImage = (c_ubyte * 4 * rgbCols * rgbRows)()
p_rgbImage = cast(rgbImage,POINTER(c_ubyte))

depthImage = (c_ushort * depthCols * depthRows)()
p_depthImage = cast(depthImage,POINTER(c_ushort))

infraredImage = (c_ushort * depthCols * depthRows)()
p_infraredImage = cast(infraredImage,POINTER(c_ushort))

bodyIndexImage = (c_ubyte * depthCols * depthRows)()
p_bodyIndexImage = cast(infraredImage,POINTER(c_ubyte))

skeleton = []
skeletonState = []
skeletonId = []
skeletonTrackingId = []

skel = (c_float * 3 * jointCount * bodyCount)()
p_skel = cast(skel,POINTER(c_float))
skelState = (c_int * jointCount * bodyCount )()
p_skelState = cast(skelState,POINTER(c_int))
skelId = (c_int * jointCount * bodyCount )()
p_skelId = cast(skelId,POINTER(c_int))
skelTrackingId = (c_uint64 * jointCount * bodyCount)()
p_skelTrackingId = cast(skelTrackingId,POINTER(c_uint64))

facePoint = []
faceRect = []
faceDirection = []
faceProperty = []
faceTrackingId = []

fcPoint = (c_float * 3 * FacePointType_Count * bodyCount)()
p_fcPoint = cast(fcPoint,POINTER(c_float))
fcRect = (c_float * 4 * bodyCount)()
p_fcRect = cast(fcRect,POINTER(c_float))
fcDirection = (c_float * 3 * bodyCount)()
p_fcDirection = cast(fcDirection,POINTER(c_float))
fcProperty = (c_int * FaceProperty_Count * bodyCount)()
p_fcProperty = cast(fcProperty,POINTER(c_int))
fcTrackingId = (c_uint64 * bodyCount)()
p_fcTrackingId = cast(fcTrackingId,POINTER(c_uint64))

hdfacePoint = (c_float * 3 * HDFaceVerticesSize * bodyCount)()
p_hdfacePoint = cast(hdfacePoint,POINTER(c_float))
hdfaceTrackingId = (c_uint64 * bodyCount)()
p_hdfaceTrackingId = cast(hdfaceTrackingId,POINTER(c_uint64))
hdfaceStatus = (c_int * 2 * bodyCount)()
p_hdfaceStatus = cast(hdfaceStatus,POINTER(c_int))

discreteGesture = []
gestureConfidence = []
discreteGestureTrackingId = []
continuousGesture = []
gestureProgress = []
continuousGestureTrackingId = []

gstId = (c_int * (100 * bodyCount))()
p_gstId = cast(gstId, POINTER(c_int))
gstFloat = (c_float * (100 * bodyCount))()
p_gstFloat = cast(gstFloat,POINTER(c_float))
gstTrackingId = (c_uint64 * (100 * bodyCount))()
p_gstTrackingId = cast(gstTrackingId,POINTER(c_uint64))


# ###################
# Python Functions
# ###################

def stopKinect():
    nt.stopKinect(kinect)
    
def imshow():
    nt.imshow(kinect)
def imshowBlack():
    nt.imshowBlack(kinect)

def mapCameraPointToColorSpace(skel,color,n):
    return nt.mapCameraPointToColorSpace(kinect,skel,color,n)
def mapCameraPointToDepthSpace(skel,color,n):
    return nt.mapCameraPointToDepthSpace(kinect,skel,color,n)
def mapDepthPointToColorSpace(depth,dth,color,n):
    return nt.mapDepthPointToColorSpace(kinect,depth,dth,color,n)
def mapDepthPointToCameraSpace(depth,dth,skel,n):
    return nt.mapDepthPointToCameraSpace(kinect,depth,dth,skel,n)

def acquire():
    nt.acquire(kinect)
def release():
    nt.release(kinect)

def setAudio(flag):
    global beamAngle, beamAngleConfidence, audioTrackingId
    nt.setAudio(kinect,flag)
    beamAngle = nt.getBeamAngle(kinect)
    beamAngleConfidence = nt.getBeamAngleConfidence(kinect)
    audioTrackingId = nt.getAudioTrackingId(kinect)
def openAudio(filename):
    nt.openAudio(kinect,filename)
def closeAudio():
    nt.closeAudio(kinect)
def isOpenedAudio():
    return nt.isOpenedAudio(kinect)

def setRGB():
    nt.setRGB(kinect)
    return getRGB()
def getRGB():
    return nt.getRGB(kinect, p_rgbImage)

def setDepth():
    nt.setDepth(kinect)
    return getDepth()
def getDepth():
    return nt.getDepth(kinect,p_depthImage)

def setInfrared():
    nt.setInfrared(kinect)
    return getInfrared()
def getInfrared():
    return nt.getInfrared(kinect,p_infraredImage)

def setBodyIndex():
    nt.setBodyIndex(kinect)
    return getBodyIndex()
def getBodyIndex():
    return nt.getBodyIndex(kinect,p_bodyIndexImage)

def setSkeleton():
    nt.setSkeleton(kinect)
    return getSkeleton()
def getSkeleton():
    global skeleton, skeletonState, skeletonId, skeletonTrackingId
    n = nt.getSkeleton(kinect,p_skel,p_skelState,p_skelId,p_skelTrackingId)
    if (n == 0):
        skeleton = []
        skeletonState = []
        skeletonId = []
        skeletonTrackingId = []
        return n
    skeleton = (c_float * 3 * jointCount * n)()
    skeletonState = (c_int * jointCount * n )()
    skeletonId = (c_int * jointCount * n )()
    skeletonTrackingId = (c_uint64 * jointCount * n)()
    for i in range(n):
        for j in range(jointCount):
            skeleton[i][j][0] = skel[i][j][0]
            skeleton[i][j][1] = skel[i][j][1]
            skeleton[i][j][2] = skel[i][j][2]
        skeletonId[i] = skelId[i]
        skeletonTrackingId[i] = skelTrackingId[i]
    return n
def setFace():
    nt.setFace(kinect, c_bool(True))
    return getFace()
faceFailCount = 0
def getFace():
    global faceFailCount, facePoint, faceRect, faceDirection, faceProperty, faceTrackingId
    n = nt.getFace(kinect,p_fcPoint,p_fcRect,p_fcDirection,p_fcProperty,p_fcTrackingId)
    if (n == 0):
        faceFailCount = faceFailCount + 1
        if faceFailCount < 10:
            return 0
        else:
            faceFailCount = 0
    facePoint = (c_float * 3 * FacePointType_Count * n)()
    faceRect = (c_float * 4 * n)()
    faceDirection = (c_float * 3 * n)()
    faceProperty = (c_int * FaceProperty_Count * n)()
    faceTrackingId = (c_uint64 * n)()
    for i in range(n):
        for j in range(FacePointType_Count):
            facePoint[i][j][0] = fcPoint[i][j][0]
            facePoint[i][j][1] = fcPoint[i][j][1]
        for j in range(4):
            faceRect[i][j] = fcRect[i][j]
        for j in range(3):
            faceDirection[i][j] = fcDirection[i][j]
        for j in range(FaceProperty_Count):
            faceProperty[i][j] = fcProperty[i][j]
        faceTrackingId[i] = fcTrackingId[i]
    return n

def setHDFace():
    nt.setHDFace(kinect)
    getHDFace()
def getHDFace():
    return nt.getHDFace(kinect,p_hdfacePoint,p_hdfaceTrackingID,p_hdfaceStatus)

def setGestureFile(filename):
    nt.setGestureFile(filename)
def setGestureId(name,id):
    return nt.setGestureId(kinect,name,id)
def setGesture():
    nt.setGesture(kinect)
def getDiscreteGesture():
    global gstId, p_gstId, gstFloat, p_gstFloat, gstTrackingId, p_gstTrackingId
    global discreteGesture, gestureConfidence, discreteGestureTrackingId
    mapSize = nt.getGidMapSize()
    size = bodyCount * mapSize
    if len(gstId) < mapSize:
        gstId = (c_int * (mapSize * bodyCount))()
        p_gstId = cast(gstId, POINTER(c_int))
        gstFloat = (c_float * (mapSize * bodyCount))()
        p_gstFloat = cast(gstFloat,POINTER(c_float))
        gstTrackingId = (c_uint64 * (mapSize * bodyCount))()
        p_gstTrackingId = cast(gstTrackingId,POINTER(c_uint64))
    n = nt.getDiscreteGesture(kinect,p_gstId,p_gstFloat,p_gstTrackingId)
    discreteGesture = (c_int * n)()
    gestureConfidence = (c_float * n)()
    discreteGestureTrackingId = (c_uint64 * n)()
    for i in range(n):
        discreteGesture[i] = gstId[i]
        gestureConfidence[i] = gstFloat[i]
        discreteGestureTrackingId[i] = gstTrackingId[i]
    return n
def getContinuousGesture():
    global gstId, p_gstId, gstFloat, p_gstFloat, gstTrackingId, p_gstTrackingId
    global continuousGesture, gestureProgress, continuousGestureTrackingId
    mapSize = nt.getGidMapSize()
    size = bodyCount * mapSize
    if len(gstId) < mapSize:
        gstId = (c_int * (mapSize * bodyCount))()
        p_gstId = cast(gstId, POINTER(c_int))
        gstFloat = (c_float * (mapSize * bodyCount))()
        p_gstFloat = cast(gstFloat,POINTER(c_float))
        gstTrackingId = (c_uint64 * (mapSize * bodyCount))()
        p_gstTrackingId = cast(gstTrackingId,POINTER(c_uint64))
    n = nt.getContinousGesture(kinect,p_gstId,p_gstFloat,p_gstTrackingId)
    continuousGesture = (c_int * n)()
    gestureProgress = (c_float * n)()
    continuousGestureTrackingId = (c_uint64 * n)()
    for i in range(n):
        continuousGesture[i] = gstId[i]
        gestureProgress[i] = gstFloat[i]
        continuousGestureTrackingId[i] = gstTrackingId[i]
    return n

def openVideo(filename):
    nt.openVideo(kinect,filename)
def writeVideo():
    nt.writeVideo(kinect)
def closeVideo():
    nt.closeVideo()

    
def doJob(n):
    for x in range(n):
        setRGB()
        setSkeleton()
        setFace()
        imshowBlack()
    stopKinect()
