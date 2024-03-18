using UnityEngine;

namespace Snake
{
    public class MapSystem : MonoSingleton<MapSystem>
    {
        public GameObject foogObj;
        float sceneHeight;
        float sceneWidth;
        float height = 16;
        float width = 30;
        protected override void OnInitializing()
        {
            base.OnInitializing();

            MapGenerator();
            FoodGenerator();
            InitEvent();
        }

        public override void ClearSingleton()
        {
            base.ClearSingleton();
            DisposeEvent();
            Destroy(transform.gameObject);
        }

        void InitEvent()
        {
            EventManager.Instance.GetFood += FoodGenerator;
        }
        void DisposeEvent()
        {
            EventManager.Instance.GetFood -= FoodGenerator;
        }

        void MapGenerator()
        {
            // 获取屏幕的宽高
            sceneHeight = Camera.main.orthographicSize * 2;
            sceneWidth = Camera.main.aspect * sceneHeight;

            height = sceneHeight - 4;
            width = (int)sceneWidth - 4;

            // Debug.Log($"scene: {sceneHeight}  {sceneWidth}");
            // Debug.Log($"map: {height}  {width}");

            // 初始化地图
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                    {
                        // 生成默认2dSprite
                        GameObject obj = Instantiate(DataManager.Instance.parfabsConfig.wallPrafabs, transform);
                        obj.name = "wall";
                        obj.transform.position = new Vector3(j - width / 2, i - height / 2, 0);
                    }
                }
            }
            DataManager.Instance.mapAABB = new Vector4(-width / 2 + 1,
                                                        -height / 2 + 1,
                                                        width - 2 - width / 2,
                                                        height - 2 - height / 2);

            // Debug.Log($"mapAABB: {DataManager.Instance.mapAABB}");
        }

        void FoodGenerator(int num = 1)
        {
            // 食物生成
            // 随机生成食物
            // 食物不能生成在蛇身上
            // 食物不能生成在墙上
            // 食物不能生成在地图边界上
            Vector2 randomPos = new Vector2(Random.Range(DataManager.Instance.mapAABB.x + 1, DataManager.Instance.mapAABB.z - 1),
                                            Random.Range(DataManager.Instance.mapAABB.y + 1, DataManager.Instance.mapAABB.w - 1));

            randomPos = new Vector2(Mathf.Round(randomPos.x) - 0.5f * Mathf.Sign(randomPos.x),
                                    Mathf.Round(randomPos.y) - 0.5f * Mathf.Sign(randomPos.y));
            if (EventManager.Instance.CheckSnakePos(randomPos))
            {
                FoodGenerator();
            }
            else
            {
                if (foogObj == null)
                {
                    foogObj = Instantiate(DataManager.Instance.parfabsConfig.foodPrafabs, transform);
                    foogObj.name = "food";
                    foogObj.transform.position = randomPos;
                    DataManager.Instance.foodPos = randomPos;
                }
                else
                {
                    foogObj.transform.position = randomPos;
                    DataManager.Instance.foodPos = randomPos;
                }
            }
        }
    }
}
