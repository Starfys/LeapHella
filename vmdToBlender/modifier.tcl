# VMD for MACOSXX86, version 1.9.2 (December 29, 2014)
# Log file '/Users/administrator/Downloads/modifier.tcl', created by user administrator
menu graphics off
menu graphics on
mol modcolor 0 0 Type
mol modstyle 0 0 Tube 0.300000 12.000000
mol modmaterial 0 0 Edgy
menu render off
menu render on
#puts [lindex $argv]
if { $argc != 1 } {
#render X3D vmdscene.x3d true
render X3D /Users/administrator/Downloads/Name.x3d true
puts 'not true'
} else {
render X3D [lindex $argv] true 
#puts 'true'
}

menu render off
# VMD for MACOSXX86, version 1.9.2 (December 29, 2014)
# end of log file.
