#!/bin/sh

# Input pictures are:
#  ../img_src/die_mz_20x.jpg:   16384x16215 pixels
#  ../img_src/die_s1_1_20x.jpg: 16384x16215 pixels
#  ../img_src/cells.png:        16384x16215 pixels
#  ../img_src/wires.png:        16384x16215 pixels
#  ../img_src/labels.png:       16384x16215 pixels
#
# This should produce:
#  die_mz_20x   with zoom level 0...6
#  die_s1_1_20x with zoom level 0...6
#  cells        with zoom level 0...6
#  wires        with zoom level 0...6
#  labels       with zoom level 0...6

set -e
TILE_SZ=256
./gen_tiles.sh ../img_src/die_mz_20x.jpg ../map/die_mz_20x $TILE_SZ
./gen_tiles.sh ../img_src/die_s1_1_20x.jpg ../map/die_s1_1_20x $TILE_SZ
./gen_tiles.sh ../img_src/cells.png ../map/cells $TILE_SZ
./gen_tiles.sh ../img_src/wires.png ../map/wires $TILE_SZ
./gen_tiles.sh ../img_src/labels.png ../map/labels $TILE_SZ
