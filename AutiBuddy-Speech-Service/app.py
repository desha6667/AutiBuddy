from flask import Flask, request, jsonify
from flask_cors import CORS
from vosk import Model, KaldiRecognizer
import wave
import json
import io
import os
 
app = Flask(__name__)
CORS(app)  # ✅ السماح للـ C# API والـ Flutter بالاتصال
 
# تحميل الموديل مرة واحدة عند بداية الـ server
MODEL_PATH = "vosk-model-small-en-us-0.15"
if not os.path.exists(MODEL_PATH):
    raise RuntimeError(f"Vosk model not found at '{MODEL_PATH}'. Download it from https://alphacephei.com/vosk/models")
 
model = Model(MODEL_PATH)
 
 
@app.route("/health", methods=["GET"])
def health():
    """نقطة للتحقق إن الـ service شغال"""
    return jsonify({"status": "ok", "model": MODEL_PATH})
 
 
@app.route("/recognize", methods=["POST"])
def recognize():
    if "file" not in request.files:
        return jsonify({"error": "No file uploaded. Send audio as 'file' field."}), 400
 
    file = request.files["file"]
 
    try:
        # قراءة الملف في الذاكرة
        audio_bytes = file.read()
        audio_io = io.BytesIO(audio_bytes)
        wf = wave.open(audio_io, "rb")
    except Exception as e:
        return jsonify({"error": f"Cannot open audio file: {str(e)}. Must be WAV format."}), 400
 
    # ✅ التحقق من مواصفات الملف
    if wf.getnchannels() != 1:
        return jsonify({"error": "Audio must be mono (1 channel). Convert stereo to mono first."}), 400
 
    if wf.getsampwidth() != 2:
        return jsonify({"error": "Audio must be 16-bit PCM."}), 400
 
    rec = KaldiRecognizer(model, wf.getframerate())
    result_text = ""
 
    while True:
        data = wf.readframes(4000)
        if len(data) == 0:
            break
        if rec.AcceptWaveform(data):
            res = json.loads(rec.Result())
            result_text += res.get("text", "") + " "
 
    final_res = json.loads(rec.FinalResult())
    result_text += final_res.get("text", "")
    result_text = result_text.strip()
 
    return jsonify({
        "text": result_text,
        "words_detected": len(result_text.split()) if result_text else 0
    })
 
 
if __name__ == "__main__":
    print(f"Starting AutiBuddy Speech Service on port 5000")
    print(f"Model: {MODEL_PATH}")
    app.run(host="0.0.0.0", port=5000, debug=False)
 