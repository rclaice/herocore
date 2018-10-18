https://0d5d3591bdfa4050b21e4ca8934e7885.us-east-1.aws.found.io:9243/

curl -k -X GET -u elasticUserName:elasticPassword https://0d5d3591bdfa4050b21e4ca8934e7885.us-east-1.aws.found.io:9243/_cat/indices


curl -H "Content-Type: application/x-ndjson" -XPOST   -u elasticUserName:elasticPasword  "https://0d5d3591bdfa4050b21e4ca8934e7885.us-east-1.aws.found.io:9243/heroes/doc/_bulk?pretty" --data-binary @heroes.json