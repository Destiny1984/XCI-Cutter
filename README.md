# XCI-Cutter

A tool to remove unused space from XCI-Dumps

=====

It seeks the end of the actual data of a XCI-Dump<br>
and cuts off everything after that point.<br><br>

Before cutting, it makes sure that the space is really unused (filled with FF).<br>
Refuses to cut if it finds something else.<br><br>

The progress is reversable.<br>
The tool can be used to add back the unused space.<br><br><br>


As a safetymeasure, the original file actually won't be touched.<br>
Everything gets copied to an output file.<br><br>

__Thanks to contributor:__<br>
* Recursive folder search for batch processing added by __getraid__.

