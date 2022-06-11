using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Firebase.Extensions;
using Firebase.Storage;
using SimpleFileBrowser;
using UnityEngine.UI;


public class LoadImage : MonoBehaviour
    {
        [SerializeField] private RawImage _image;
        [SerializeField] private FireBase _fireBase;
        private Button _imageButton;
        
        private void Start()
        {
            _imageButton = GetComponent<Button>();
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Images",
                ".jpg", ".png"));
            FileBrowser.SetDefaultFilter(".jpg");
            FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
            
            _imageButton.onClick.AddListener(ImageButtonClick);
        }
    
        private void ImageButtonClick()
        {
            PickImage(512);
        }
    
        private void PickImage( int maxSize )
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
            {
                Debug.Log( "Image path: " + path );
                if( path != null )
                {
                    CurrentGame.ImageName = Path.GetFileName(path);
                    Debug.Log("Name of file -" + CurrentGame.ImageName);
                
                    Texture2D texture = NativeGallery.LoadImageAtPath( path);
                    if( texture == null )
                    {
                        Debug.Log( "Couldn't load texture from " + path );
                        return;
                    }
                    
                    GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );
    
                    Material material = quad.GetComponent<Renderer>().material;
                    if( !material.shader.isSupported ) 
                        material.shader = Shader.Find( "Legacy Shaders/Diffuse" );
    
                    _image.texture = texture;
    
                   CurrentGame.ImageData= texture.GetRawTextureData();
                    
                    Destroy( quad );
                }
            } );
    
            Debug.Log( "Permission result: " + permission );
        }
    }
