using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class LoopBackground : MonoBehaviour {

	public Sprite[] sprites;

	public int fps = 6;
	
	public bool playOnAwake = true;
	
	public bool loop = true;
	
	public bool destroyOnEnd = false;
	
	public UnityEvent onAnimationComplete;

	private bool m_Play = false;

	private int m_Index = 0;

	private Image m_Image;
	
	private int m_Frame = 0;

	void Awake() {
		m_Image = GetComponent<Image> ();
		PlayOnAwake();
	}

	public void Play() {
		EnableImage();
		m_Index = 1;
		m_Play = true;
	}

	public void Stop() {
		DisableImage();
		m_Index = 0;
		m_Play = false;
	}

	void Update () {
		if (!m_Play) {
			return;
		}

		m_Frame ++;
		if (m_Frame < fps) {
			return;
		}
		m_Frame = 0;


		if (m_Index >= sprites.Length) {
			OnAnimationComplete();
		}

		m_Image.sprite = sprites [m_Index];
		m_Index ++;
	}

	void OnAnimationComplete() {
		m_Index = 0;
		onAnimationComplete.Invoke();

		if(destroyOnEnd) {
			Destroy(gameObject);
		} else if(!loop) {
			Stop();
		}
	}

	void PlayOnAwake() {
		if(!playOnAwake) {
			DisableImage();
		} else {
			Play();
		}
	}

	void EnableImage() {
		if(!m_Image.enabled) {
			m_Image.enabled = true;
		}
	}

	void DisableImage() {
		if(m_Image.enabled) {
			m_Image.enabled = false;
		}
	}
}