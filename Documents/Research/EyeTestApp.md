# Eye Test App

## App Features

* Direction detection - direction of E/C letters in various distance and angle of a letter.

* Light Detection - detection of area with light on the rhombus grid as function of the grid distance. 


## Procedure

* When measuring visual acuity, it is often desirable to measure the worst of :
  * [Prompt] the two eyes first 
  * [Prompt] the eye with the better acuity
  * [Camera] Purple size dilation, to record entrance pupil size at the time of measurement. 

* It is desirable to record entrance pupil size at the time of measurement. If vision is poor, it is often useful to evaluate visual acuity using an artificial pupil of small dimensions.


* Special presentation conditions may include: 
  * unusual projection methods; 
  * response time limits; 
  * chromatic backgrounds; 
  * low contrast testing; 
  * testing at very low
  * very high luminance levels;
  * testing with single letters
  * variable degrees of "crowding."

* Four separate optotype orientations: 
  * up, 
  * down, 
  * right
  * left

* Ratio is to be` 1:100.1` or `1:1.2589`. This ratio is also referred to as 0.1 log unit (base10). 

* Prepare the optotypes so that their dimensions follow the precisely specified geometric progression, rather than the approximate values used by the clinician. 

* It is highly desirable that chart designers or manufacturers provide at least 5 optotype presentations of each size, to be displayed on a single line, if possible.

* XI.4 To specify the letter height of typeset materials in M-units the following approximations may be useful: 1M = 1.5mm (actually 1.454), 1M = 1/16 inch (actually 0.92/16) or `M-rating = mm size x 0.7` (actually 0.69).

* For example, 0.40/1 (in meters) or 40/100 (in centimeters) indicates the ability to read 1M print at 40 cm and would be equivalent to a distance visual acuity (measured at 4 m) of 4/10. 

* Each optotype is to be exposed for no longer than `3 seconds` with a judgment period of up to `4 seconds` between exposures.

* Dimension Data designed for presentation at 4 meters 
  
  Gab size Dimensions to be Used for the Preparation of Landolt Rings for use at 4 m

  ```py
  # Size of Gap and Width of Stroke
  StrokeGab_mm = [0.50, 0.63, 0.79, 1.00, 1.26, 1.59, 2.00, 2.51, 3.16, 3.98, 5.01, 6.31, 7.94, 10.00]
  StrokeWidth_mm = [0.58, 0.73, 0.92, 1.16, 1.46, 1.84, 2.34, 2.92, 3.68, 4.63, 5.83, 7.34, 9.24, 11.64]

  # Notations to Designate Optotypes (in M-units = distance in m at which gap subtends 1 min of arc)
  MUnit_Use_m = [2.00, 2.52, 3.18, 4.00, 5.04, 6.34, 7.98, 10.05, 12.56, 15.92, 20.05, 25.24, 31.77, 40.00]
  
  # gab in arc minutes
  Acuity = 1 / gab

  # Snellen Notation
  ## test distance in meters
  face-device-distance
  ## letter size in M-units
  MUnit_Use_m
  ## acuity
  Acuity = face-device-distance / MUnit_Use_m
  ```

* Six, five and four meters have been used by different groups of practitioners, but few examining rooms are built for a 6-meter.


## Analysis

The visual acuity score of an individual should express the reciprocal of the angular size of the critical detail within the smallest optotype that can be correctly recognized by that individual. 

Health subjects have 6/6 vision or "better" (20/15, 20/10, etc.) 

