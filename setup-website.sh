#!/bin/bash

# EasyValidate Website Setup Script
# This script clones and sets up the external EasyValidate documentation website repository

WEBSITE_REPO_URL="https://github.com/mu-dawood/EasyValidate-Website.git"  # Update this URL
WEBSITE_DIR="Website"

echo "🚀 Setting up EasyValidate Documentation Website..."

# Check if the website directory already exists
if [ -d "$WEBSITE_DIR" ]; then
    echo "📁 Website directory already exists. Pulling latest changes..."
    cd "$WEBSITE_DIR"
    git pull origin main
    cd ..
else
    echo "📥 Cloning EasyValidate Website repository..."
    git clone "$WEBSITE_REPO_URL" "$WEBSITE_DIR"
fi

# Check if clone/pull was successful
if [ $? -eq 0 ]; then
    echo "✅ Website repository setup complete!"
    echo "📂 Website is available in: ./$WEBSITE_DIR"
    echo ""
    echo "To start development:"
    echo "  cd $WEBSITE_DIR"
    echo "  npm install"
    echo "  npm run dev"
    echo ""
    echo "To build for production:"
    echo "  cd $WEBSITE_DIR"
    echo "  npm run build"
else
    echo "❌ Failed to setup website repository"
    echo "Please check the repository URL and your internet connection"
    exit 1
fi
