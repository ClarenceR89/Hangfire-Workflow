# Hangfire + Workflow Core Playground:

- Idea is to utilise Hangfire queues to schedule many work items, and to have each work item picked up and processed by a worker, each worker has workflow-core installed on it.
    - Services will be external to the workers, with recipes for remote execution built into the workers at compile time. (eg. restful, socket, lambda etc.) this way services can be dynamically added to the server's
    - another option would be to create workflow(redis) aware services

### Dockerised Hangfire Dashboard: Contains API for Queueing work and for adding definitions to Workflow Redis

### Dockerised Workers - clustered: Connects to Redis to handle Hangfire queues, and internal to queue workers we will schedule workflow items
- Workflow and hangfire share a single Redis as backplane

### Dockerised Worker Management API for aws/azure, to scale out should queue get to full

## TODO:
- Unit tests
- Add Service connections: Implementations to call external micro-services
- Definition API: CRUD for Workflow definitions
- Queue API: for (De|En)queueing workloads
- Cron API: For scheduling workloads
- Dockerise:
    - Worker Docker(Workers are passed the Queue's they must work in hangfire)
    - Dashboard Docker: For monitoring Hangfire Queues
    - Investigate ways to:
        - Connect to rest
        - Connect to workflow events
        - Connect to lambdas
        - Connect to azure functions
        - Connect to sockets
