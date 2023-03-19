using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GradientObject<T>
{
    public GradientType type;
    public T gradientObj;
    public float speed;
    public float waitTime;
    public CircularQueue<Color> _colorPallete;

    public IEnumerator Gradient(){
        yield return new WaitForSeconds(waitTime);

        float timer = 0f;
        float lerpTime = 0f;
        Color currentColor = (type == GradientType.Camera ? (gradientObj as Camera).backgroundColor : (gradientObj as SpriteRenderer).color);
        Color nextColor = _colorPallete.Dequeue();

        while (true)
        {
            lerpTime += Time.deltaTime * speed;

            if(type == GradientType.Camera){
                (gradientObj as Camera).backgroundColor = Color.Lerp(currentColor, nextColor, lerpTime);
            }
            else if(type == GradientType.SkyLight){
                (gradientObj as SpriteRenderer).color = Color.Lerp(currentColor, nextColor, lerpTime);
            }

            if((type == GradientType.Camera ? (gradientObj as Camera).backgroundColor : (gradientObj as SpriteRenderer).color).Equals(nextColor))
            {
                timer += Time.deltaTime;
            }

            if (timer > waitTime)
            {
                timer = 0f;
                lerpTime = 0f;
                currentColor = (type == GradientType.Camera ? (gradientObj as Camera).backgroundColor : (gradientObj as SpriteRenderer).color);
                nextColor = _colorPallete.Dequeue();
            }

            yield return null;
        }
    }
}
