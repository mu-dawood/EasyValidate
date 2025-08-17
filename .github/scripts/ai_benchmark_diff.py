#!/usr/bin/env python3
"""
ai_benchmark_diff.py

Compare two benchmark CSV files and generate a summary of differences using Gemini AI.
Highlights regressions, improvements, and negligible changes.
"""

import argparse
import difflib
import sys
from pathlib import Path
import google.generativeai as genai
import os


def read_file(path: Path) -> list[str]:
    """Read file and return lines."""
    try:
        return path.read_text().splitlines(keepends=True)
    except Exception as e:
        sys.exit(f"Error reading {path}: {e}")



def generate_diff(prev_content: list[str], cur_content: list[str], prev_path: Path, cur_path: Path) -> str:
    diff_lines = difflib.unified_diff(
        prev_content, cur_content,
        fromfile=str(prev_path),
        tofile=str(cur_path)
    )
    return ''.join(diff_lines)


def summarize_with_gemini(prev_content: list[str], cur_content: list[str]) -> str:
    api_key = os.environ.get("GEMINI_API_KEY")
    if not api_key:
        sys.exit("Error: GEMINI_API_KEY environment variable is required")

    genai.configure(api_key=api_key)
    model = genai.GenerativeModel("gemini-1.5-flash")

    prompt = f"""
You are a benchmark analysis assistant.

Highlight:
- Regressions (slower)
- Improvements (faster)
- Negligible changes

Deep focus on EasyValidate

Compare the following two benchmark CSV files:

Previous CSV:
{''.join(prev_content)}

Current CSV:
{''.join(cur_content)}
"""
    response = model.generate_content(prompt)
    return response.text


def main():
    parser = argparse.ArgumentParser(description="Compare two benchmark CSV files and summarize differences using Gemini AI.")
    parser.add_argument("prev_file", type=Path, help="Path to previous benchmark CSV file")
    parser.add_argument("current_file", type=Path, help="Path to current benchmark CSV file")
    parser.add_argument("--show-diff", action="store_true", help="Show raw diff output before summary")
    args = parser.parse_args()

    prev_content = read_file(args.prev_file)
    cur_content = read_file(args.current_file)


    if args.show_diff:
        diff_content = generate_diff(prev_content, cur_content, args.prev_file, args.current_file)
        if diff_content:
            print("\n## Raw Diff\n")
            print(diff_content)
        else:
            print("\n(No differences found)\n")

    summary = summarize_with_gemini(prev_content, cur_content)

    print("\n## AI Benchmark Diff Summary\n")
    print(summary)


if __name__ == "__main__":
    main()
