using System.IO;
using Unity.Netcode;
using UnityEngine;


[System.Serializable]
public struct HumanHeadSettings
{
    //head
    public float skullSize;

    public float jawWidth;
    public float jawHeight;
    public float jawDepth;

    public float cheekboneWidth;
    public float cheekboneHeight;
    public float cheekSize;

    public float chinLength;
    public float chinWidth;

    //eyes
    public float eyeHeight;
    public float eyeDistance;
    public float eyeDepth;
    public float eyeSize;

    //brows
    public float outerBrow;
    public float innerBrow;
    public float browDistance;
    public float browDepth;

    //mouth
    public float mouthWidth;
    public float mouthHeight;
    public float lipSize;

    //nose
    public float noseWidth;
    public float noseHeight;
    public float noseDepth;

    //ears
    public float earRotation;
    public float earHeight;
    public float earSize;

    //additions
    public int hairStyle;
    public int browStyle;
    public int sideBeardStyle;
    public int stacheStyle;
    public int beardStyle;

    //colors

    public Color eyeLidColor;
    public Color scleraColor;
    public Color irisColor;
    public Color pupilColor;
    public Color makeupColor;
    public Color lipColor;
}

[System.Serializable]
public struct HumanBodySettings
{
    public HumanHeadSettings headSettings;

    public float upperNeckLength;
    public float upperNeckWidth;
    public float lowerNeckWidth;

    public float atlasLength;
    public float torsoDepth;

    public float torsoWidth;
    public float ribLength;
    public float waist;
    public float bellyLength;
    public float shoulderWidth;
    public float shoulderSize;
    public float armLength;
    public float elbowSize;
    public float forearmLength;
    public float wristSize;

    public float upperHipWidth;
    public float upperHipRadius;
    public float lowerHipRadius;
    public float hipLength;
    public float hipOutRotation;

    public float thighLength;
    public float kneeRadius;
    public float upperCalfLength;
    public float calfRadius;
    public float lowerCalfLength;
    public float ankleRadius;

    public Color skinColor;
    public Color hairColor;

    public Material basicInGameObject;

    public HumanBodyCoverageSettings coverSettings;

    public float length;
}

[System.Serializable]
public struct HumanBodyCoverageSettings
{
    public Color color;
    public bool upperNeck;
    public bool lowerNeck;
    public bool shoulders;
    public bool elbows;
    public bool wrists;
    public bool chest;
    public bool waist;
    public bool hips;
    public bool butt;
    public bool knees;
    public bool calves;
    public bool ankles;
}