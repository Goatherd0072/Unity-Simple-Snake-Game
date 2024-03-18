using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class SnakeController : MonoSingleton<SnakeController>
    {
        public GameObject snakeHead;
        // public GameObject snakeTail;
        public Queue<GameObject> snakebody = new();
        public int snakeInitLength = 5;
        public int snakeLength = 5;
        public int toAddLength = 0;
        public WaitForSeconds wfSpeed = new(0.5f);
        public EMoveDirction direction = EMoveDirction.RIGHT;
        public bool isDead = false;
        Coroutine moveCoroutine;

        override protected void OnInitializing()
        {
            base.OnInitializing();
            snakeLength = snakeInitLength;
            //随机方向
            // direction = (EMoveDirction)Random.Range(1000, 1004);
            InitSnake();
            InitEvent();
            moveCoroutine = StartCoroutine(SnakeMove());
        }

        public override void ClearSingleton()
        {
            base.ClearSingleton();
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            DisposeEvent();
            Destroy(transform.gameObject);
        }

        private void Update()
        {
            if (isDead)
            {
                return;
            }

            ChangeDirection();
            // UpdateSnakeBody();
            CheckCollider();
        }

        IEnumerator SnakeMove()
        {
            while (!isDead)
            {
                yield return wfSpeed;
                // if (Random.Range(0f, 1f) > 0.5f)
                // {
                //     AddSnakeBody();
                // }
                // Debug.Log($"SnakeMove");
                Vector3 newPos = GetMovePos();
                snakeHead = snakebody.Dequeue();
                snakebody.Enqueue(snakeHead);
                snakeHead.transform.position = newPos;

                // UpdateSnakeBody();
                // CheckCollider();
                // if (Random.Range(0f, 1f) > 0.5f)
                // {
                //     AddSnakeBody();
                // }
            }
        }
        Vector3 GetMovePos()
        {
            Vector3 ToMovePos = snakeHead.transform.position;
            switch (direction)
            {
                case EMoveDirction.UP:
                    ToMovePos.y += 1;
                    break;
                case EMoveDirction.DOWN:
                    ToMovePos.y -= 1;
                    break;
                case EMoveDirction.LEFT:
                    ToMovePos.x -= 1;
                    break;
                case EMoveDirction.RIGHT:
                    ToMovePos.x += 1;
                    break;
            }
            return ToMovePos;
        }

        void InitSnake()
        {
            //初始化蛇头
            snakeHead = Instantiate(DataManager.Instance.parfabsConfig.snakePrafab, new Vector3(-0.5f, -0.5f, 0), Quaternion.identity, transform);

            Vector3 SpawnPos = snakeHead.transform.position;
            SpawnPos.x -= (snakeInitLength - 1) * 1;

            //初始化蛇身
            for (int i = 0; i < snakeInitLength - 1; i++)
            {
                snakebody.Enqueue(Instantiate(DataManager.Instance.parfabsConfig.snakePrafab, SpawnPos, Quaternion.identity, transform));
                SpawnPos.x += 1;
            }
            snakebody.Enqueue(snakeHead);

            // int inu = 0;
            // foreach (var go in snakebody)
            // {
            //     go.name = inu.ToString();
            //     inu++;
            // }
            // Debug.Log(snakebody.Peek().name);
        }

        void InitEvent()
        {
            EventManager.Instance.GetFood += AddSnakeLength;
            EventManager.Instance.CheckSnakePos += CheckSnakePos;
        }
        void DisposeEvent()
        {
            EventManager.Instance.GetFood -= AddSnakeLength;
            EventManager.Instance.CheckSnakePos -= CheckSnakePos;
        }

        void AddSnakeLength(int length)
        {
            // toAddLength += length;
            while (length > 0)
            {
                snakeLength++;
                snakeHead = Instantiate(DataManager.Instance.parfabsConfig.snakePrafab, GetMovePos(), Quaternion.identity, transform);
                snakebody.Enqueue(snakeHead);
                length--;
            }
        }

        void UpdateSnakeBody()
        {
            if (toAddLength == 0)
                return;

            while (toAddLength > 0)
            {
                snakeLength++;
                snakeHead = Instantiate(DataManager.Instance.parfabsConfig.snakePrafab, GetMovePos(), Quaternion.identity, transform);
                snakebody.Enqueue(snakeHead);
                toAddLength--;
            }
        }

        void ChangeDirection()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeDirection(EMoveDirction.UP);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeDirection(EMoveDirction.LEFT);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeDirection(EMoveDirction.DOWN);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeDirection(EMoveDirction.RIGHT);
            }
        }

        //防止反向移动
        void ChangeDirection(EMoveDirction newDirection)
        {
            if (Mathf.Abs((int)direction - (int)newDirection) == 1)
            {
                return;
            }
            direction = newDirection;
        }

        void CheckCollider()
        {
            Vector3 headPos = snakeHead.transform.position;

            foreach (var go in snakebody)
            {
                if (go == snakeHead)
                {
                    continue;
                }

                if (go.transform.position == headPos)
                {
                    // Debug.Log("die");
                    DeadEvent();
                }
            }

            if (headPos.x < DataManager.Instance.mapAABB.x ||
                headPos.x > DataManager.Instance.mapAABB.z ||
                headPos.y < DataManager.Instance.mapAABB.y ||
                headPos.y > DataManager.Instance.mapAABB.w)
            {
                // Debug.Log("die");
                DeadEvent();
            }

            if (headPos.x == DataManager.Instance.foodPos.x &&
                headPos.y == DataManager.Instance.foodPos.y)
            {
                EventManager.Instance.GetFood(1);
            }
        }

        void DeadEvent()
        {
            isDead = true;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = null;
            EventManager.Instance.GameOver?.Invoke();
        }

        bool CheckSnakePos(Vector2 pos)
        {
            foreach (var go in snakebody)
            {
                if (go.transform.position.x == pos.x && go.transform.position.y == pos.y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
