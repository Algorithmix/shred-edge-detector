shred-edge-detector
===================

program to determine whether an image of a shred belongs to the edge of a page



At this programs current stage, it will load an image from file location and then output the average 
variance of both sides of the shred in a console window.  Only the middle 50% of the shred is analyzed to 
prevent the ends from obscuring the data.



How Does it Work?

First, a canny edge detection is run on the shred.  Then the program iterates through each row of the edge detection
output recording the first and last encounters.  

NOTE: THIS PROGRAM ASSUMES THAT THE SHRED IMAGE USED HAS NO NOISE OUTSIDE OF THE SHRED PARIMETER

The x coordinates are cached in a queue (size 10) so that the mean X-coordinate can be updated with the current
state of the edge.  The recorded x coordinate minus the local mean x coordinate squared is what I am calling "variance".

This seems to be a good indicator of the smoothness of the edge.  In theory, abnormally small variance 
will indicate smoothness of the edge of the paper and it has proven successful in the 2 trials that I've done.



To Be Done:

  - Determine a proper threshold for the variance
