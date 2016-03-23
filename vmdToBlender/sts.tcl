# VMD for MACOSXX86, version 1.9.2 (December 29, 2014)
# Log file '/Users/administrator/Downloads/sts.tcl', created by user administrator
mol addfile {/Users/administrator/Downloads/bound.pdb} type {pdb} first 0 last -1 step 1 waitfor 1 0
animate style Loop
menu graphics off
menu graphics on
menu files off
mol color Type
mol representation DynamicBonds 1.600000 0.300000 12.000000
mol selection all
mol material Edgy
mol modrep 0 0
menu render off
menu render on
render X3D /Users/administrator/Downloads/newrender.x3d true
mol modstyle 0 0 Bonds 0.300000 12.000000
render X3D /Users/administrator/Downloads/newrender.x3d true
render STL /Users/administrator/Downloads/ssss.stl true
