Tasks:
# 1. Create Dashboard Project
 - Use the `angular` or `react-redux` with Dotnet template
 - use a proper css/scss framework eg. Bootstrap, Material or tailwind

# 2. To the dashboard project add SwaggerUI
 - in order to visualise and quickly test API endpoints

# 3. Hangfire Dashboard
 - To the dashboard project add a hangfire dashboard

# 4. Workflow Views And Controller
 - Add a Webapi controller to allow you to manage workflows
 - Add to the project created in step 1 a view to visualise and control the current workflow states eg. Start, Stop, Pause
 - Add a form to start the hello world workflow
 - Add an additional view to see the state of the workflow by ID, eg see what is happening in the current steps
 - Workflows should only be processed on the worker and as such some interfaces have been provided on the Hangfire.Lib project that must be implemented

# 5. Additional: Dockerise the Dashboard and the Worker
 - Must start via compose
