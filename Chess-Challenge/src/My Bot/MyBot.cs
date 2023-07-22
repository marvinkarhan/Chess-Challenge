using System;
using ChessChallenge.API;

public class MyBot : IChessBot
{
  public Move Think(Board board, Timer timer)
  {
    return Search(board, 1, 4, -200000, 200000).Item2;
  }

  // node == 1 is PV node
  private (int, Move) Search(Board board, int node, int depth, int alpha, int beta)
  {
    Move bestMove = new();
    if (depth == 0) return (Evaluate(board), bestMove);
    Move[] moves = board.GetLegalMoves();
    int score = 200000;
    int best = -200000;
    int madeMoves = 0;
    bool pvNode = node == 1;

    foreach (Move move in moves)
    {
      madeMoves++;
      board.MakeMove(move);
      if (!pvNode || madeMoves > 1) {
        score = -Search(board, 0, depth - 1, -alpha - 1, -alpha).Item1;
      }
      if (pvNode && ((score > alpha && score < beta) || madeMoves == 1))
      {
        score = -Search(board, 1, depth - 1, -beta, -alpha).Item1;
      }
      board.UndoMove(move);
      // cutoff
      if (score > best)
      {
        best = score;

        if (score > alpha)
        {
          alpha = score;
          bestMove = move;
        }
      }
    }
    // check for mate / draw
    if (madeMoves == 0) best = board.IsInCheck() ? board.PlyCount - 200000 : 0;

    if (pvNode) best = Math.Min(best, 200000);
    return (best, bestMove);
  }

  int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

  private int Evaluate(Board board)
  {
    int score = 0;
    PieceList[] pieceLists = board.GetAllPieceLists();
    foreach (PieceList pieces in pieceLists)
    {
      foreach (Piece piece in pieces)
      {
        score += piece.IsWhite ? pieceValues[(int)piece.PieceType] : -pieceValues[(int)piece.PieceType];
      }
    }
    return board.IsWhiteToMove ? score : -score;
  }
}