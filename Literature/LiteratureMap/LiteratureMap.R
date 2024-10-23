library(igraph)
library(ggraph)
library(ggplot2)

nodes <- read.csv("nodes.csv", stringsAsFactors = FALSE)
edges <- read.csv("edges.csv", stringsAsFactors = FALSE)

graph <- graph_from_data_frame(d = edges, vertices = nodes, directed = TRUE)

ggraph(graph, layout = "kk") +
  geom_edge_link(
    aes(width = weight), color = "grey", 
    arrow = arrow(length = unit(4, 'mm')), 
    end_cap = circle(7, 'mm'), 
    show.legend = FALSE
  ) +
  scale_edge_width(range = c(0.1, 0.5)) +
  geom_node_point(aes(color = type), size = 10) +  # Color nodes by 'type'
  geom_node_text(aes(label = name), size = 3, vjust = 0.5, hjust = 0.5) +  # Center label in node
  scale_color_manual(values = c("violet", "green", "red")) +  # Assign specific colors to types
  labs(color = "Category") +  # Change the legend title from 'type' to 'Category'
  theme_void()