# XCI-Cutter
A tool to remove unused space from XCI-Dumps

It seeks the end of the actual data of a XCI-Dump
and cuts off everything after that point.

Before cutting, it makes sure that the space is really unused (filled with FF).
Refuses to cut if it finds something else.

The progress is reversable.
The tool can be used to add back the unused space.


As a safetymeasure, the original file actually won't be touched.
Everything gets copied to an output file.

# Edit
In this version I added a recursive folder search under batch processing
