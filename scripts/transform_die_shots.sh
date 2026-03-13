#!/bin/sh

# Get die shots from https://siliconpr0n.org/map/nintendo/dmg-cpu-b/single/

set -e

magick ../img_src/nintendo_dmg-cpu-b_mcmaster_mz_mit20x.jpg \
       -filter     triangle \
       -resize     16384x \
       -background white \
       -gravity    south \
       -extent     16384x16215 \
       -gravity    center \
       -extent     16384x16384 \
       ../img_src/die_mz_20x.png

magick ../img_src/nintendo_dmg-cpu-b_mcmaster_s1-1_mit20x.jpg \
       -filter     triangle \
       -resize     16302x14783! \
       -background white \
       -gravity    northwest \
       -extent     16384x16215-39-1440 \
       -gravity    center \
       -extent     16384x16384 \
       ../img_src/die_s1_1_20x.png
