from fastapi import FastAPI
from pydantic import BaseModel
import sqlite3

app = FastAPI()


class ScoreData(BaseModel):
    name: str
    score: int


# DB 초기화 (ID, 이름, 점수, 시간)
def init_db():
    conn = sqlite3.connect("game.db")
    cursor = conn.cursor()
    cursor.execute(
        """
        CREATE TABLE IF NOT EXISTS scores (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT,
            score INTEGER,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        )
    """
    )
    conn.commit()
    conn.close()


init_db()


# 1. 점수 저장 (Upsert)
@app.post("/save-score")
async def save_score(data: ScoreData):
    conn = sqlite3.connect("game.db")
    cursor = conn.cursor()

    # 기존 기록이 있는지 확인 (Select)
    cursor.execute("SELECT score FROM scores WHERE name = ?", (data.name,))
    result = cursor.fetchone()

    if result:
        # 기존 기록이 있을 때: 점수 비교
        old_score = result[0]
        if data.score > old_score:
            # 새 점수가 더 높을 때만 업데이트 (Update)
            cursor.execute(
                "UPDATE scores SET score = ?, created_at = CURRENT_TIMESTAMP WHERE name = ?",
                (data.score, data.name),
            )
            msg = f"최고 기록 갱신! ({old_score} -> {data.score})"
        else:
            # 점수가 더 낮거나 같으면 아무것도 안 함
            msg = f"기존 기록({old_score})이 더 높습니다. 유지합니다."
    else:
        # 기존 기록이 없을 때: 새로 추가 (Insert)
        cursor.execute(
            "INSERT INTO scores (name, score) VALUES (?, ?)", (data.name, data.score)
        )
        msg = f"{data.name}님 첫 등록 완료!"

    conn.commit()
    conn.close()
    return {"status": "success", "message": msg}


# 2. 리더보드 조회 (Top 5)
@app.get("/leaderboard")
async def get_scores():
    conn = sqlite3.connect("game.db")
    cursor = conn.cursor()
    cursor.execute("SELECT name, score FROM scores ORDER BY score DESC LIMIT 5")
    rows = cursor.fetchall()
    conn.close()

    return [
        {"rank": i, "name": row[0], "score": row[1]} for i, row in enumerate(rows, 1)
    ]
