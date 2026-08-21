using Flavor;
using UnityEngine;

public class BoardSpawner : BaseMono
{
    [Header("Grid Visual")]
    [SerializeField] private GameObject _cellGridPrefab;
    [SerializeField] private GameObject _groundGridPrefab;
    [SerializeField] private Transform _gridParent;

    protected override void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        ServiceLocator.TryGet<BoardSystem>(out var boardSystem);
        SpawnVisualGrid(boardSystem._width, boardSystem._height);
    }

    private void SpawnVisualGrid(int width, int height)
    {
        if (_cellGridPrefab == null || _gridParent == null) return;
        var yCell = 0.5f;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(_cellGridPrefab);
                cell.transform.SetParent(_gridParent, false);
                cell.transform.position = new Vector3(x, yCell, y);
                cell.name = $"Grid_{x}_{y}";
            }
        }
        var yGround = 0f;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject ground = Instantiate(_groundGridPrefab);
                ground.transform.SetParent(_gridParent, false);
                ground.transform.position = new Vector3(x, yGround, y);
                ground.name = $"Ground_{x}_{y}";
            }
        }
        // Lấy BoxCollider ra, nếu chưa có thì Add thêm vào (để tránh bị add nhiều lần nếu spawn lại)
        BoxCollider boardCollider = _gridParent.GetComponent<BoxCollider>();
        if (boardCollider == null)
        {
            boardCollider = _gridParent.gameObject.AddComponent<BoxCollider>();
        }
        // Kích thước mâm bằng đúng width và height
        boardCollider.size = new Vector3(width, 1f, height);

        // Vị trí tâm (Công thức: lấy ô cuối cùng trừ đi ô đầu tiên rồi chia đôi)
        // Lưu ý: Y = 0 để cho nó trùng với mặt phẳng yGround
        boardCollider.center = new Vector3((width - 1) / 2f, 0f, (height - 1) / 2f);

    }


}
