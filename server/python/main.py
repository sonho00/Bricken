from fastapi import FastAPI, Body
from sqlalchemy import create_engine, text
from contextlib import asynccontextmanager
import uvicorn

DATABASE_URL = "sqlite:///./scores.db"
engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})


@asynccontextmanager
async def lifespan(app: FastAPI):
    with engine.connect() as conn:
        conn.execute(
            text(
                """--sql
            CREATE TABLE IF NOT EXISTS scores (
                name TEXT PRIMARY KEY,
                score INTEGER NOT NULL,
                ts DATETIME DEFAULT CURRENT_TIMESTAMP
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
    yield


app = FastAPI(lifespan=lifespan)


@app.post("/post-score")
async def post_score(name: str = Body(...), score: int = Body(...)):
    with engine.connect() as conn:
        query = text(
            """--sql
            INSERT INTO scores (name, score) VALUES (:n, :s)
            ON CONFLICT(name) DO UPDATE SET
            score = :s WHERE :s > scores.score
        """
        )
        conn.execute(query, {"n": name, "s": score})
        conn.commit()
    return {"status": "success"}


@app.get("/get-scores")
async def get_scores():
    with engine.connect() as conn:
        res = conn.execute(
            text("SELECT name, score FROM scores ORDER BY score DESC LIMIT 5")
        )
        ranks = [{"name": r[0], "score": r[1]} for r in res]
    return {"ranks": ranks}


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
