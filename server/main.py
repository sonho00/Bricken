from sqlalchemy import create_engine, text
from sqlalchemy.pool import QueuePool
import uvicorn
from fastapi import FastAPI, Depends
from pydantic import BaseModel
from datetime import datetime

app = FastAPI()

DATABASE_URL = "sqlite:///./scores.db"
engine = create_engine(
    DATABASE_URL,
    connect_args={"check_same_thread": False},
    poolclass=QueuePool,
    pool_size=10,
    max_overflow=5,
    pool_timeout=30,
)


class Score(BaseModel):
    name: str
    score: int


def get_db():
    conn = engine.connect()
    try:
        yield conn
    finally:
        conn.close()


def init_db():
    with engine.connect() as conn:
        conn.execute(
            text(
                """--sql
            CREATE TABLE IF NOT EXISTS scores (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name VARCHAR(100) UNIQUE,
                score INTEGER,
                timestamp TIMESTAMP
            )
        """
            )
        )
        conn.execute(
            text(
                """--sql
            CREATE INDEX IF NOT EXISTS idx_scores_score_desc ON scores(score DESC)
        """
            )
        )
        conn.commit()


@app.post("/post-score")
def post_score(score: Score, db=Depends(get_db)):
    query = text(
        """--sql
        INSERT INTO scores (name, score, timestamp) VALUES (:name, :score, :ts)
        ON CONFLICT(name) DO UPDATE SET
            score = excluded.score,
            timestamp = excluded.timestamp
        WHERE excluded.score > scores.score
    """
    )
    result = db.execute(
        query, {"name": score.name, "score": score.score, "ts": datetime.now()}
    )
    db.commit()

    if result.rowcount > 0:
        return {"status": "Success"}
    return {"status": "Low score"}


@app.get("/get-scores")
def get_scores(db=Depends(get_db)):
    query = text(
        """--sql
        SELECT name, score FROM scores ORDER BY score DESC LIMIT 5
    """
    )
    results = db.execute(query).fetchall()
    return [{"name": r[0], "score": r[1]} for r in results]


if __name__ == "__main__":
    init_db()
    uvicorn.run(app, host="0.0.0.0", port=8000)
