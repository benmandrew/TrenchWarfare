using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingTextureManager : MonoBehaviour
{

    public GameObject texturePrefab;
    public List<GameObject> textureObjectList;
    private int spriteNum;
    private float spriteSize;
    private int firstSpriteIndex;
    public float textureSpeedAbs;
    private float textureSpeed;
    private GameObject cam;
    public float cameraBuffer;
    public bool isMoving;
    private Vector3 tempPosition;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        spriteSize = 10.0f;
        spriteNum = -1;
        int[] directionArray = new int[] { -1, 1 };
        textureSpeed = textureSpeedAbs * directionArray[Random.Range(0, 2)];
        textureObjectList = fillScreenWithTexture();
    }

    List<GameObject> fillScreenWithTexture()
    {
        GameObject texture;
        List<GameObject> textureObjects = new List<GameObject>();
        // Make a texture sprite at startingX Fully cover the left side of the camera's view
        float startingX = transform.position.x;
        do
        {
            startingX -= spriteSize + spriteSize + spriteSize / 2.0f;
        } while (startingX - spriteSize / 2.0f >= cam.transform.position.x - (cam.GetComponent<Camera>().aspect * spriteSize) / 2.0f);
        float spawnX = startingX - spriteSize;
        do
        {
            spawnX += spriteSize;
            spriteNum += 1;
            texture = createTextureInstance(spawnX);
            texture.transform.parent = transform;
            textureObjects.Add(texture);
        } while (spawnX + spriteSize / 2.0f <= cam.transform.position.x + (cam.GetComponent<Camera>().aspect * spriteSize) / 2.0f);
        return textureObjects;
    }

    GameObject createTextureInstance(float x)
    {
        GameObject textureObject = Instantiate(texturePrefab);
        textureObject.transform.position = new Vector3(x, 0.0f, 0.0f);
        return textureObject;
    }

    // Update is called once per frame
    void Update()
    {
        shiftTexture();

        tempPosition = transform.position;
        if (isMoving)
        {
            tempPosition.x += textureSpeed;
        }
        transform.position = tempPosition;
    }

    void shiftTexture()
    {
        // Camera moved left
        if (cam.transform.position.x - ((cam.GetComponent<Camera>().aspect * spriteSize) / 2.0f + cameraBuffer) < textureObjectList[firstSpriteIndex].transform.position.x - spriteSize / 2.0f)
        {
            rotateTextureList(-1, firstSpriteIndex);
            firstSpriteIndex = getIndex(firstSpriteIndex + 1, textureObjectList.Count);
        }
        // Camera moved right
        else if (cam.transform.position.x + ((cam.GetComponent<Camera>().aspect * spriteSize) / 2.0f + cameraBuffer) > textureObjectList[getIndex(firstSpriteIndex - 1, textureObjectList.Count)].transform.position.x + spriteSize / 2.0f)
        {
            rotateTextureList(1, firstSpriteIndex);
            firstSpriteIndex = getIndex(firstSpriteIndex - 1, textureObjectList.Count);
        }
    }

    void rotateTextureList(int direction, int firstSpriteIndex)
    {
        int lastSpriteIndex;
        GameObject texture;
        Vector3 newPosition;
        if (direction == -1)
        {
            lastSpriteIndex = getIndex(firstSpriteIndex - 1, textureObjectList.Count);
            texture = textureObjectList[lastSpriteIndex];
            newPosition = textureObjectList[firstSpriteIndex].transform.position;
            newPosition.x -= spriteSize;
            texture.transform.position = newPosition;
        }
        else if (direction == 1)
        {
            lastSpriteIndex = getIndex(firstSpriteIndex - 1, textureObjectList.Count);
            texture = textureObjectList[firstSpriteIndex];
            newPosition = textureObjectList[lastSpriteIndex].transform.position;
            newPosition.x += spriteSize;
            texture.transform.position = newPosition;
        }
    }

    int getIndex(int index, int listSize)
    {
        // C# is stupid when it comes to the modulus operator
        // So I had to write my own one
        // https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
        return (index % listSize + listSize) % listSize;
    }
}
