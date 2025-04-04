#!/bin/bash

set -e

SRC_IMG=$1
OUT_DIR=$2
TILE_SZ=$3

if [ -z "$SRC_IMG" -o -z "$OUT_DIR" -o -z "$TILE_SZ" ] ||
   ![ "$TILE_SZ" -eq "$TILE_SZ" ] 2>/dev/null ||
   [[ TILE_SZ -le 0 ]]; then
	echo "Usage: ./gen_tiles.sh <source_img> <out_dir> <tile_size>" >&2
	exit 1
fi

SRC_IMG=$(realpath -es "$SRC_IMG")
SRC_EXT=${SRC_IMG##*.}
if [ "$SRC_IMG" = "$SRC_EXT" ]; then
	SRC_EXT=
else
	SRC_EXT=.${SRC_EXT}
fi

mkdir -p "$OUT_DIR"
cd "$OUT_DIR"
if [ -z "$(find . -maxdepth 0 -type d -empty 2>/dev/null)" ]; then
	echo Output directory not empty >&2
	exit 1
fi

SRC_IMG_W=$(identify -format %w "$SRC_IMG")
SRC_IMG_H=$(identify -format %h "$SRC_IMG")

echo Source image size: ${SRC_IMG_W}x${SRC_IMG_H}

SZ_MAX=$(( (SRC_IMG_W > SRC_IMG_H) ? SRC_IMG_W : SRC_IMG_H ))

MAX_ZOOM=0
LVL_SZ[0]=$TILE_SZ
LVL_TLW[0]=1
LVL_TLC[0]=1

while (( LVL_SZ[MAX_ZOOM] < SZ_MAX )); do
	(( MAX_ZOOM += 1 ))
	LVL_SZ[$MAX_ZOOM]=$(( LVL_SZ[MAX_ZOOM-1] * 2 ))
	LVL_TLW[$MAX_ZOOM]=$(( LVL_TLW[MAX_ZOOM-1] * 2 ))
	LVL_TLC[$MAX_ZOOM]=$(( LVL_TLC[MAX_ZOOM-1] * 4 ))
done

echo Max zoom level: $MAX_ZOOM
echo Max zoom image size: ${LVL_SZ[MAX_ZOOM]}x${LVL_SZ[MAX_ZOOM]}

for (( i = 0; i <= MAX_ZOOM; i++ )); do
	for (( j = 0; j < LVL_TLW[i]; j++ )); do
		mkdir -p "$i/$j"
	done

	magick "$SRC_IMG" -filter     triangle \
	                  -resize     ${LVL_SZ[i]}x${LVL_SZ[i]} \
	                  -gravity    center \
	                  -background "rgba(255, 255, 255, 0)" \
	                  -extent     ${LVL_SZ[i]}x${LVL_SZ[i]} \
	                  +gravity \
	                  -crop       ${TILE_SZ}x${TILE_SZ} \
	       "$i/tmp-%d$SRC_EXT"

	for (( j = 0; j < LVL_TLC[i]; j++ )); do
		mv "$i/tmp-$j$SRC_EXT" "$i/$(( j / LVL_TLW[i] ))/$(( j % LVL_TLW[i] ))$SRC_EXT"
	done
done
