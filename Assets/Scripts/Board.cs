using UnityEngine.Tilemaps;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Tilemap tileMap {get; private set;}
    public Piece activePiece { get; private set;}
    [SerializeField]
    private GameController gameController;
    public TetrominoData[] tetrominoes;
    public Vector3Int spwanPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);

    public int LinesCleared {get; private set;}

    private string levelKey = "Level";


    public RectInt Bounds{
        get{
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake(){

        this.tileMap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        LinesCleared = 0;

        for(int i = 0; i< this.tetrominoes.Length; i++){
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start(){
        SpawnPiece();
    }

    public void SpawnPiece(){
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spwanPosition, data);
        if(IsValidPosition(this.activePiece, this.spwanPosition)){
            Set(this.activePiece);
        }else{
            GameOver();
        }
    }

    private void GameOver(){
        this.tileMap.ClearAllTiles();
        PlayerPrefs.SetInt(levelKey, 0);
        this.gameController.GameOver();
    }
    
    public void Set(Piece piece){
        for(int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tileMap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece){
        for(int i = 0; i < piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tileMap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition( Piece piece, Vector3Int position){

        RectInt bounds = this.Bounds;

        for(int i = 0; i< piece.cells.Length; i++){
            Vector3Int tilePosition = piece.cells[i] + position;

            if(!bounds.Contains((Vector2Int)tilePosition)){
                return false;
            }

            if(this.tileMap.HasTile(tilePosition)){
                return false;
            }
        }

        return true;
    }

    public void ClearLines(){
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax){
            if(IsLineFull(row)){
                LineClear(row);
                this.LinesCleared++;
            }else{
                row++;
            }
        }

        if(LinesCleared == 1) this.gameController.AddPoints(100);
        if(LinesCleared == 2) this.gameController.AddPoints(300);
        if(LinesCleared == 3) this.gameController.AddPoints(500);
        if(LinesCleared > 3) this.gameController.AddPoints(800);

        LinesCleared = 0;
    }

    private bool IsLineFull(int row){
        RectInt bounds = this.Bounds;

        for(int coll = bounds.xMin; coll < bounds.xMax; coll++){
            Vector3Int position = new Vector3Int(coll, row, 0);

            if(!this.tileMap.HasTile(position)){
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row){
        RectInt bounds = this.Bounds;

        for(int coll = bounds.xMin; coll < bounds.xMax; coll++){
            Vector3Int position = new Vector3Int(coll, row, 0);

            this.tileMap.SetTile(position, null);
        }

        while(row < bounds.yMax){
            for(int coll = bounds.xMin; coll < bounds.xMax; coll++){
                Vector3Int position = new Vector3Int(coll, row + 1, 0);

                TileBase above = this.tileMap.GetTile(position);

                position = new Vector3Int(coll, row, 0);
                this.tileMap.SetTile(position, above);
            }

            row++;
        }
    }
}
