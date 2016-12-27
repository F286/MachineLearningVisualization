using UnityEngine;
using System.Collections;

public class CONTENT_Display : MonoBehaviour 
{
    public virtual float value
    {
        set
        {
            _value += value;
            _valueCount++;
        }
    }
    public float derivative
    {
        set
        {
            _derivative += value;
            _derivativeCount++;
        }
    }
    public SpriteRenderer sprite;
    protected float _value;
    protected int _valueCount;
    protected float _derivative;
    protected int _derivativeCount;

    public void Awake()
    {
        sprite = gameObject.AddComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>("Box Neuron");
        value = 0.5f;
        sprite.sortingOrder = -5;
    }
    public virtual void LateUpdate()
    {
        if (_valueCount > 0)
        {
            _value /= _valueCount;

            var c = CONTENT_ManagerNeuron.instance.gradient.Evaluate(Mathf.InverseLerp(-1, 1, _value));
            sprite.color = c;
            var scale = 0.05f + (1f / (1f + Mathf.Exp(-Mathf.Abs(_value / 5)))) * 0.3f;
            scale *= 2;
            transform.localScale = new Vector3(scale, scale, 1);

            _valueCount = 0;
            _value = 0;
        }
        if (_derivativeCount > 0)
        {
            _derivative /= _derivativeCount;

            var r = transform.localEulerAngles;
            r.z += _derivative * -20 * (1f / 60f) * 10;
            transform.localEulerAngles = r;

            _derivativeCount = 0;
            _derivative = 0;
        }
    }
}
