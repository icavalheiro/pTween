using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;


public class TestScript : MonoBehaviour {
	public Transform from;
	public Transform to;
	public AnimationCurve curve;

	private void Start() {
		StartCoroutine(Animate());
	}

	IEnumerator Animate(){
		Debug.Log("começou animação");

		//execute tween
		yield return Tween.Interval(2, (t) => {
			transform.position = Tween.Lerp(from.position, to.position, t);// AnimationEase.CUBIC_IN);//initPos + (delta * t);
		}, curve);
		
		//that's it folks
		Debug.Log("terminou animação");
	}
}
