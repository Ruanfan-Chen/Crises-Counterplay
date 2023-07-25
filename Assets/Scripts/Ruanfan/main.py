from __future__ import print_function
from flask import Flask

import traceback
import requests
import os

app = Flask(__name__)

SCOPES = "https://www.googleapis.com/auth/forms.responses.readonly"
DISCOVERY_DOC = "https://forms.googleapis.com/$discovery/rest?version=v1"

url = "https://docs.google.com/forms/d/e/1FAIpQLSf0TWlv15XbhbF5PsAkbB6y1YpulvTrGBAQcdO-ep-aAtTnvQ/viewanalytics?pli=1&pli=1"


@app.route("/",  methods=['GET'])
def hello_world():
    return "hello world"


@app.route("/getScores",  methods=['GET'])
def get_scores():
    scores = []
    try:
        response = requests.get(url)
        tmp = response.text.split("[575622577,[")
        t2 = tmp[1].split(",0]],")[0]
        print()
        idx = t2.index("]]")
        print("res = ", t2[:idx+1])
        l = t2[:idx+1][1:-1].split("],[")
        print("l =", l)
        for item in l:
            print(item)
            scores.append(float(item.split(",")[0][1:-1]))
        print("scores = ", scores)
    except:
        traceback.print_exc()
    scores.sort()
    return ",".join([str(s) for s in scores])


if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=int(os.environ.get("PORT", 8080)))
    # [[null, [null, [[1397740063, "Session ID", null, 0, [[1776650237, null, 1]], null, null, null, null, null, null, [null, "Session ID"]], [389481172, "Score (time in seconds)", null, 0, [[575622577, null, 1]], null, null, null, null, null, null, [null, "Score (time in seconds)"]]], ["Your response has been recorded", 0, 1, 0, 0], null, null, [0, 0], null, null, "Trinity - Infinite Mode Score", 66, [null, 0, null, 2, 0, null, 1], null, null, null, null, [2], null, null, null, null, null, null, null, null, null, [null, "Trinity - Infinite Mode Score"]], "/forms", "Trinity Score Time", null, null, null, "", null, 0, 0, null, "University of Southern California", 1, "e/1FAIpQLSf0TWlv15XbhbF5PsAkbB6y1YpulvTrGBAQcdO-ep-aAtTnvQ", 0, "[]", 0, 0], null, null, [[1776650237, [["638258224278310670", null, 2], ["638258355533723050", null, 2], ["638258243830828130", null, 1], ["638258377011716840", null, 1], ["12345", null, 1], ["2382344982", null, 1]], 8, 0], [575622577, [["9.410203", null, 1], ["9.3386", null, 1], ["19.43798", null, 1], ["41.40971", null, 1], ["24.67816", null, 1], ["22.74837", null, 1], ["18.231", null, 1], ["25.81", null, 1]], 8, 0]], null, 8, null, [[0, 8]], null, null, null, null, [[1776650237, ["5c502034207c1757", [null, 0, "#673ab7"]]]]];
