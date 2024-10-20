## load package ggplot2 (sudo R -e "install.packages('ggplot2')")
library("ggplot2")



## load the data
dat = read.csv("height.csv")

# Variables
weight = dat$Weight
height = dat$Height
shoeSize = dat$ShoeSize



## Plots
# Boxplot
boxplot(shoeSize,
        col = "orange",
        ylab= "Value",
        xlab= "Shoe Size"
        )

boxplot(height,
        col = "blue",
        ylab= "Value",
        xlab= "Height"
        )


# Scatter plot and line of best fit - Alt1
plot(height, shoeSize)
abline(lm(shoeSize ~ height))

# Scatter plot and line of best fit - Alt2
ggplot(data = dat,
       aes(x= height,
           y=shoeSize,
       )
)  +
  # Scatter plot
  geom_point(color = "darkgrey") +
  # Fitted line (linear):
  # The gray area along the line is the confidence interval.
  # If gray area is irrelevant, it may be removed with the 
  # extra argument se=FALSE
  geom_smooth(method="lm",
              col="black",
              #se=FALSE
  )



### Control assumptions
## Normal Distribution Tests
##  Shapiro-Wilk normality test for the first group
shapiro.test(shoeSize)
##  Shapiro-Wilk normality test for the second group
shapiro.test(height)

## Variance Tests
## Fisher's F test to compare the two variances
var.test(shoeSize, height)

### Run correlation Tests
## We ignore Pearson's Correlation Test since the data is not normally distributed
#cor.test(height, shoeSize, method="pearson")

## We also ignore Kendall's Correlation Test since the dataset is quite large
#cor.test(height, shoeSize, method="kendall")

## Perform Spearman's Correlation Test
cor.test(height, shoeSize, method="spearman")