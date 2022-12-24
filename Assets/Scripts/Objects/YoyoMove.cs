using UnityEngine;

namespace Objects
{
    public static class YoyoMove
    {
        public static void HorizontalMove(float startPos, float endPos, float speed, Rigidbody2D rb, float time)
        {
            Vector2 vec2StartPos = new Vector2(startPos, rb.position.y);
            Vector2 vec2EndPos = new Vector2(endPos, rb.position.y);

            float distance = Vector2.Distance(vec2StartPos, vec2EndPos) / 2;
            Vector2 position = new Vector2(((vec2StartPos.x + vec2EndPos.x) / 2) + Mathf.Sin(speed * time) * distance, vec2StartPos.y);

            rb.MovePosition(position);
        }
    }
}