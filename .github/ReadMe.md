
# Using GitHub Copilot

These instruction, prompt and chatmodes are a work in progress
Any file starting with underscore (_) is not ready to be used!

This project is intended to test any improve the prompts, instructions and chatmodes.
The idea is that by starting with a new project (no legacy code).
The instructions and documentation are the only input provided to Copilot.
That way I can evaluate the result an try to improve the instruction where needed.

## Instructions

Instructions will be extended inline with the project structure. The current setup was copied from another project an serves as starting point. The goal is to target (apply) the instruction only when they are relevant for the current task.

For example: Unit test instructions should only be provided to copilot when the current task requires the creation of unit tests.
(which is probably always, but you get the idea).

### ARC 42

While I will try to make the instruction files reusable for multiple projects, Copilot will also need require project specific information. My view is that information should be specified according ARC 42. For each chapter there is an ACR 42 instruction file and a prompt file to generate the chapter. I'm attempting to replace the prompt files with the ARC 42 chatmode. With the current setup the chatmode uses the prompt files, but **it may be better to merge the prompt an the instruction file for each chapter**.

Additionally in the general copilot-instructions.md I included that the `doc\arc42\` should be included while gathering information.

## Prompts

The goal for this project is to develop most features using the generate-plan and implement plan prompts.
I have been using them with some success. My hope is that by providing better instruction files and starting with a clean (no legacy code) project I can improve the success rate.

## Chat modes

The current chatmodes are used for specification and documentation only. Is use them in Visual Studio code. Development I mainly do in Visual Studio (which at this moment does not support chat modes).

Because these chatmodes are used the create the specifications there may be some conflicts with the instructions.
**I intent to improve on that by checking the used references**.
