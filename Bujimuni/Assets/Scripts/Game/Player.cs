using System;
using UnityEngine;
using QFramework;
using Unity.Mathematics;

namespace Bujimuni
{
	public partial class Player : ViewController
	{
		private Sprite UpSprite;
		private Sprite DownSprite;
		private Sprite LeftSprite;
		void Start()
		{
			"Hello Bujimuni!".LogInfo();
			var bujiSprite = Resources.LoadAll<Sprite>("Texture/buji");
			UpSprite = bujiSprite[2];
			DownSprite = bujiSprite[0];
			LeftSprite = bujiSprite[1];
		}

		private void Update()
		{
			//Move
			if (Input.GetKey(KeyCode.W))
			{
				transform.position += transform.up * Time.deltaTime * 10;
				Square.sprite = UpSprite;
			}
			if (Input.GetKey(KeyCode.S))
			{
				transform.position -= transform.up * Time.deltaTime * 10;
				Square.sprite = DownSprite;
			}
			if (Input.GetKey(KeyCode.A))
			{
				transform.position -= transform.right * Time.deltaTime * 10;
				Square.sprite = LeftSprite;
				transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			if (Input.GetKey(KeyCode.D))
			{
				transform.position += transform.right * Time.deltaTime * 10;
				Square.sprite = LeftSprite;
				transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
		}
	}
}
