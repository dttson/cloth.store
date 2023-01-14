using System;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace ClothStore.Character
{
    public class CharacterController : MonoBehaviour
    {
        public event Action<GameObject> onHitOtherObject;
        public Vector2 MoveDirection => _moveDirection;
        
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private BoxCollider2D _boxCollider;

        private Vector2 _moveDirection;
        private float xMin, xMax;
        private float yMin, yMax;

        void Start()
        {
            var characterSize = _boxCollider.size;
            var worldBounds = GameManager.Instance.getGameWorldBounds();
            xMin = worldBounds.min.x + characterSize.x / 2;
            xMax = worldBounds.max.x - characterSize.x / 2;
            
            yMin = worldBounds.min.y;
            yMax = worldBounds.max.y - characterSize.y;
        }

        void Update()
        {
            if (GameManager.Instance.UIManager.isAnyUIOpen())
            {
                _moveDirection = Vector2.zero;
                return;
            }
            
            _moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            _moveDirection *= _moveSpeed * Time.deltaTime;

            var newPosition = transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x + _moveDirection.x, xMin, xMax);
            newPosition.y = Mathf.Clamp(newPosition.y + _moveDirection.y, yMin, yMax);
            transform.position = newPosition;
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
            onHitOtherObject?.Invoke(col.gameObject);
        }
    }
}