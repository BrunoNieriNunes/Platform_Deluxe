using System.Runtime.CompilerServices;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private float z;

    [SerializeField] private Vector2 parallaxFactor;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool infiniteHorizontal;
    [SerializeField] private bool infiniteVertical;
    [SerializeField] private Transform cam;

    private void Start() {
        this.z = this.transform.position.z;

        this.lastCameraPosition = this.cam.position;
        Vector3 startPosition = new Vector3(this.lastCameraPosition.x + this.offset.x, this.lastCameraPosition.y + this.offset.y, this.z);
        this.transform.position = startPosition;

        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        this.textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * this.transform.localScale.x;
        this.textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * this.transform.localScale.y;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = this.cam.position - this.lastCameraPosition;
        this.transform.position += new Vector3(deltaMovement.x * this.parallaxFactor.x, deltaMovement.y * this.parallaxFactor.y, 0);
        this.lastCameraPosition = this.cam.position;

        float cameraTravelDistanceX = this.cam.position.x - this.transform.position.x;
        float cameraTravelDistanceY = this.cam.position.y - this.transform.position.y;

        if (this.infiniteHorizontal && Mathf.Abs(cameraTravelDistanceX) >= this.textureUnitSizeX) {
            float offsetPositionX = cameraTravelDistanceX % this.textureUnitSizeX;
            this.transform.position = new Vector3(this.cam.position.x + offsetPositionX + this.offset.x, this.transform.position.y, this.z);
        }

        if (this.infiniteVertical && Mathf.Abs(cameraTravelDistanceY) >= this.textureUnitSizeY) {
            float offsetPositionY = cameraTravelDistanceY % this.textureUnitSizeY;
            this.transform.position = new Vector3(this.transform.position.x, this.cam.position.y + offsetPositionY + this.offset.y, this.z);
        }
    }
}