# Figures api test demo app

To start just buld project and run it. It will open Swagger UI page for API examination.

## Advantages

This demo shows next technologies:

1. Custom Text.Json converter for polymorphic deserialization/serialization, the subtype is determined through property name of json object.
2. Swashbuckle powered. Includes example as Xml comments for POST /api/figure.
3. SOLID principles, it is easy to add new geometry figure - we should not touch other classes.
4. Unit tests use inMemory db.

## Assumptions

1. Geometry data is stored as serialized string.

# Api examples

## POST a new fihure

### Circle:

    POST /api/figure
    {
        "circle": 
        {
            "center": {"x": 2, "y": 2},
            "radius": 10
        }
    }

### Triangle:

    POST /api/figure
    {
        "triangle": 
        {
            "a": {"x": 0, "y": 0},
            "b": {"x": 0, "y": 3},
            "c": {"x": 4, "y": 0}
        }
    }

## GET a figure square

    GET /api/figure/1
