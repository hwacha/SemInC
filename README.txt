To build, type ./build into the terminal.
To run, type ./run into the terminal.

Please don't add a main to any class other than Testing.

https://en.wikipedia.org/wiki/C-sharp_minor#Notable_songs

TODO:
  - add resolve inconsistencies
    - need probabilities
    - need algorithm that cascades down justification
    - observation sentences are default justified
  - split model into beliefs, desires, and intentions
  - start making models for NPCs/common knowledge
  - make an XML parser which handles logical forms and model information (could just make it all logical forms, as in: make all these sentences true in the model.)
  - make interesting scenarios based on the results!

DONE:
  - add rule update
    - with potential space saving by not expanding into S-Rules
    - F-Rules field
    - S-Rules field
    - active rules field, for rules that have led to an inference
      - not checked every update
      - only checked after an inconsistency
      - also serves as record of justification
    - 1. expand F-Rules into S-Rules
    - 2. When a new entity of type T gets added to the model, check
      the F-Rules to add another S-Rules
    - 3. Check all S-Rules every update, iterate until there's no change
    - 4. If an S-Rule successfully infers something, but it in the active rules field
    - 5. if not, keep it in the S-Rules field.
  - add submodels which encode other's beliefs, desires, intentions
